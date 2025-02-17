using System;
using DG.Tweening;
using NGame;
using NGame.Camera;
using NGame.UI;
using NShared.Board;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace NPlayer
{
	public class PlayerController : IDisposable
	{
		private readonly PlayerObject playerObject;
		private readonly UIHandler uiHandler;
		private readonly BoardManager boardManager;
		private readonly CameraAnchor cameraAnchor;

		private Tile currentPlayerTile;

		[Inject]
		public PlayerController(
			PlayerObject playerObject,
			UIHandler uiHandler,
			BoardManager boardManager,
			CameraAnchor cameraAnchor
		) {
			this.playerObject = playerObject;
			this.uiHandler = uiHandler;
			this.boardManager = boardManager;
			this.cameraAnchor = cameraAnchor;

			boardManager.OnBoardLoaded += OnBoardLoaded;
			uiHandler.OnTravelButtonClicked += OnTravelButtonClicked;
		}

		private void OnBoardLoaded() {
			currentPlayerTile = boardManager.BoardData.GetStartTile();
			SetPlayerPositionOnTile(currentPlayerTile);
		}

		private void OnTravelButtonClicked() {

			int roll = Random.Range(1, 11);

			Tile startTile = currentPlayerTile;
			Tile endTile = boardManager.BoardData.GetNextTile(startTile, roll);

			MovePlayerTo(startTile, endTile);
			MoveCameraAlongPath(startTile, endTile);
		}

		private void MovePlayerTo(Tile startTile, Tile endTile) {

			Vector3[] path = boardManager.BoardData.GetNextTilePath(startTile, endTile);
			cameraAnchor.transform.DOPath(path, path.Length, PathType.CatmullRom, PathMode.Full3D);

			MovePlayerAlongPath(startTile, endTile);
		}

		private void MovePlayerAlongPath(Tile startTile, Tile endTile) {
			Vector3[] path = boardManager.BoardData.GetNextTilePath(startTile, endTile);
			playerObject.transform
				.DOPath(path, path.Length, PathType.CatmullRom, PathMode.Full3D)
				.SetEase(Ease.Linear)
				.OnWaypointChange(w => Debug.Log(w))
				.OnComplete(() => SetPlayerPositionOnTile(endTile));
		}

		private void MoveCameraAlongPath(Tile startTile, Tile endTile) {
			Vector3[] path = boardManager.BoardData.GetNextTilePath(startTile, endTile);
			cameraAnchor.transform
				.DOPath(path, path.Length, PathType.CatmullRom, PathMode.Full3D)
				.SetEase(Ease.Linear);
		}

		private void SetPlayerPositionOnTile(Tile tile) {
			Vector3 tilePosition = tile.TileObject.gameObject.transform.position;
			playerObject.transform.position = new Vector3(tilePosition.x, 0.3f, tilePosition.z);
			currentPlayerTile = tile;
		}

		public void Dispose() {
			boardManager.OnBoardLoaded -= OnBoardLoaded;
			uiHandler.OnTravelButtonClicked -= OnTravelButtonClicked;
		}
	}
}