using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using NShared;
using NShared.Board;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace NGame
{
	public class BoardManager
	{
		private readonly TileTypeList tileTypeList;
		private readonly Transform boardParent;
		public BoardData BoardData { get; } = new();

		private const float TilePositionRange = 0.02f;
		private const float TileHeightBase = -0.17f;
		private const float TileHeightRange = 0.02f;
		private const float TileRotationRange = 4;
		private const float TileTiltRange = 2;

		public event Action OnBoardLoaded;

		[Inject]
		public BoardManager(string boardFileName, TileTypeList tileTypeList, Transform boardParent) {
			this.tileTypeList = tileTypeList;
			this.boardParent = boardParent;

			LoadBoardLayout(boardFileName);
		}

		private async UniTask<TileObject> LoadTilePrefab() {
			return await Addressable.Load<TileObject>("Board/Tile");
		}

		private void LoadBoardLayout(string boardFileName) {

			string boardFile = Path.Combine(BoardData.BoardsFolderPath, $"{boardFileName}.json");

			string json = File.ReadAllText(boardFile);
			BoardData loadedBoardData = JsonUtility.FromJson<BoardData>(json);
			loadedBoardData.RestoreAfterDeserialization();

			CreateBoard(loadedBoardData).Forget();
		}

		private async UniTaskVoid CreateBoard(BoardData loadedBoardData) {

			TileObject tilePrefab = await LoadTilePrefab();

			foreach (Tile loadedTile in loadedBoardData.Tiles) {
				AddTile(CreateTile(tilePrefab, loadedTile.Position, loadedTile.TileType));
			}

			Tile startTile = BoardData.GetStartTile();
			boardParent.position -= new Vector3(startTile.Position.x, 0, startTile.Position.z);

			OnBoardLoaded?.Invoke();
		}

		private Tile CreateTile(TileObject tilePrefab, Vector3Int gridPosition, TileTypeEnum selectedTileType) {

			Vector3 placementPosition = gridPosition + (Vector3.one * Random.Range(-TilePositionRange, TilePositionRange));
			placementPosition.y = TileHeightBase + Random.Range(-TileHeightRange, TileHeightRange);

			Quaternion placementRotation = Quaternion.Euler(
				Random.Range(-TileTiltRange, TileTiltRange),
				Random.Range(-TileRotationRange, TileRotationRange),
				Random.Range(-TileTiltRange, TileTiltRange)
			);

			TileObject tileObject = Object.Instantiate(tilePrefab, placementPosition, placementRotation);
			tileObject.transform.SetParent(boardParent);
			return new Tile(gridPosition, selectedTileType, tileTypeList.GetMaterial(selectedTileType), tileObject);
		}

		private bool AddTile(Tile tile) {
			bool uniqueTile = BoardData.Tiles.Add(tile);
			if (uniqueTile) {
				BoardData.OrderedTiles ??= new List<Tile>();
				BoardData.OrderedTiles.Add(tile);
			}
			return uniqueTile;
		}
	}
}