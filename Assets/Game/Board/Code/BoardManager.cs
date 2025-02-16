using System.IO;
using Cysharp.Threading.Tasks;
using NBoardEditor;
using NShared;
using NShared.Board;
using UnityEngine;
using Zenject;

namespace NGame
{
	public class BoardManager
	{
		private readonly TileTypeList tileTypeList;
		private readonly Transform boardParent;
		private BoardData BoardData { get; } = new();

		private float tilePositionRange = 0.02f;
		private float tileHeightBase = -0.3f;
		private float tileHeightRange = 0.02f;
		private float tileRotationRange = 4;
		private float tileTiltRange = 2;

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
		}

		private Tile CreateTile(TileObject tilePrefab, Vector3Int gridPosition, TileTypeEnum selectedTileType) {

			Vector3 placementPosition = gridPosition + (Vector3.one * Random.Range(-tilePositionRange, tilePositionRange));
			placementPosition.y = tileHeightBase + Random.Range(-tileHeightRange, tileHeightRange);

			Quaternion placementRotation = Quaternion.Euler(
				Random.Range(-tileTiltRange, tileTiltRange),
				Random.Range(-tileRotationRange, tileRotationRange),
				Random.Range(-tileTiltRange, tileTiltRange)
			);

			TileObject tileObject = Object.Instantiate(tilePrefab, placementPosition, placementRotation);
			tileObject.transform.SetParent(boardParent);
			return new Tile(gridPosition, selectedTileType, tileTypeList.GetMaterial(selectedTileType), tileObject);
		}

		private bool AddTile(Tile tile) {
			return BoardData.Tiles.Add(tile);
		}
	}
}