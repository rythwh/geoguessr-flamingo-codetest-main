using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NGame;
using NGame.Camera;
using NGame.UI;
using NShared;
using NShared.Board;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NPlayer
{
	public class PlayerController : IDisposable
	{
		private readonly PlayerObject playerObject;
		private readonly UIHandler uiHandler;
		private readonly BoardManager boardManager;
		private readonly CameraAnchor cameraAnchor;
		private readonly PlayerProfile playerProfile;

		private FloatingText floatingTextPrefab;

		private Tile currentPlayerTile;

		private const float moveTweenDuration = 0.3f;
		private bool blockInput = false;

		public event Action<Tile> OnPlayerMovedToTile;

		[Inject]
		public PlayerController(
			PlayerObject playerObject,
			UIHandler uiHandler,
			BoardManager boardManager,
			CameraAnchor cameraAnchor,
			PlayerProfile playerProfile
		) {
			this.playerObject = playerObject;
			this.uiHandler = uiHandler;
			this.boardManager = boardManager;
			this.cameraAnchor = cameraAnchor;
			this.playerProfile = playerProfile;

			LoadFloatingText().Forget();

			boardManager.OnBoardLoaded += OnBoardLoaded;
			uiHandler.OnTravelButtonClicked += OnTravelButtonClicked;
		}

		private async UniTaskVoid LoadFloatingText() {
			floatingTextPrefab = await Addressable.Load<FloatingText>("Player/FloatingText");
		}

		private void OnBoardLoaded() {
			currentPlayerTile = boardManager.BoardData.GetStartTile();
			SetPlayerPositionOnTile(currentPlayerTile);
		}

		private void OnTravelButtonClicked() {

			if (blockInput) {
				return;
			}

			int roll = Random.Range(1, 11);
			Debug.Log($"Rolled {roll}");

			Tile startTile = currentPlayerTile;
			Tile endTile = boardManager.BoardData.GetNextTile(startTile, roll);

			MovePlayerTo(startTile, endTile);
			MoveCameraAlongPath(startTile, endTile);
		}

		public void MovePlayerTo(Tile startTile, Tile endTile) {
			SetInputBlock(true);
			List<Tile> path = boardManager.BoardData.GetNextTilePath(startTile, endTile);
			playerObject.Animator.enabled = true;
			MovePlayerToTilePosition(path, 0);

		}

		private void MovePlayerToTilePosition(List<Tile> path, int index) {
			if (index >= path.Count) {
				return;
			}

			Tile tile = path[index];

			playerObject.transform
				.DOJump(GetPathPointPosition(tile), 0.5f, 1, moveTweenDuration);
			playerObject.transform
				.DOMoveX(GetPathPointPosition(tile).x, moveTweenDuration)
				.OnComplete(() => OnCompletePlayerMove(path, index));
			playerObject.transform
				.DOMoveZ(GetPathPointPosition(tile).z, moveTweenDuration);

			playerObject.Animator.CrossFade(playerObject.PlayerJumpingAnimation, 1f);

			Material tileMaterial = tile.TileObject.Renderer.material;
			Color tileColour = tileMaterial.color * 1.5f;

			tileMaterial.DOColor(tileColour, moveTweenDuration / 2f)
				.SetDelay(moveTweenDuration)
				.SetLoops(2, LoopType.Yoyo);

			Vector3 tileButtonPosition = tile.TileObject.TileButtonTransform.position;
			tile.TileObject.TileButtonTransform
				.DOJump(tileButtonPosition, -0.1f, 1, moveTweenDuration / 2f)
				.SetDelay(moveTweenDuration);
		}

		private Vector3 GetPathPointPosition(Tile tile) {
			Vector3 pathPoint = tile.TileObject.gameObject.transform.position;
			pathPoint.y = 0.3f;
			return pathPoint;
		}

		private void OnCompletePlayerMove(List<Tile> path, int pathIndex) {

			if (path[pathIndex].TileType == TileTypeEnum.Start) {
				EvaluateLandedTile(path[pathIndex]);
			}

			pathIndex += 1;
			if (pathIndex >= path.Count) {
				Tile tile = path[^1];
				playerObject.Animator.CrossFade(playerObject.PlayerIdleAnimation, 1f);
				SetPlayerPositionOnTile(tile);
				SetInputBlock(false);
				OnPlayerMovedToTile?.Invoke(tile);
				EvaluateLandedTile(tile);
				return;
			}
			MovePlayerToTilePosition(path, pathIndex);
		}

		private void SetInputBlock(bool state) {
			blockInput = state;
			uiHandler.OnInputBlockChanged?.Invoke(blockInput);
		}

		private void MoveCameraAlongPath(Tile startTile, Tile endTile) {
			Vector3[] path = boardManager.BoardData
				.GetNextTilePath(startTile, endTile)
				.Select(t => t.TileObject.gameObject.transform.position)
				.ToArray();
			cameraAnchor.transform
				.DOPath(path, path.Length * moveTweenDuration, PathType.CatmullRom, PathMode.Full3D)
				.SetEase(Ease.Linear);
		}

		private void SetPlayerPositionOnTile(Tile tile) {
			Vector3 tilePosition = tile.TileObject.gameObject.transform.position;
			playerObject.transform.position = new Vector3(tilePosition.x, 0.3f, tilePosition.z);
			currentPlayerTile = tile;
		}

		private void EvaluateLandedTile(Tile tile) {
			switch (tile.TileType) {
				case TileTypeEnum.Empty:
					playerProfile.AddCoins(1000);
					CreateFloatingText("+1000").Forget();
					break;
				case TileTypeEnum.Start:
					playerProfile.AddCoins(10000);
					CreateFloatingText("+10000").Forget();
					break;
				case TileTypeEnum.Quiz:
					break;
				case TileTypeEnum.QuizFlag:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private async UniTask CreateFloatingText(string text) {
			await UniTask.WaitUntil(() => floatingTextPrefab != null);
			FloatingText floatingText = Object.Instantiate(
				floatingTextPrefab,
				playerObject.transform.position + (Vector3.up * 1.5f),
				Quaternion.identity
			);
			floatingText.SetText(text);
			await UniTask.WaitForSeconds(2);
			Object.Destroy(floatingText.gameObject);
		}

		public void Dispose() {
			boardManager.OnBoardLoaded -= OnBoardLoaded;
			uiHandler.OnTravelButtonClicked -= OnTravelButtonClicked;
		}
	}
}