using System.IO;
using Cysharp.Threading.Tasks;
using NShared;
using NShared.Board;
using UnityEngine;
using Zenject;

namespace NGame
{
	public class BoardManager
	{
		private BoardData BoardData { get; } = new();

		[Inject]
		public BoardManager(string boardFileName) {
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
		}

		private Tile CreateTile(TileObject tilePrefab, Vector3Int gridPosition, TileTypeEnum selectedTileType) {
			TileObject tileObject = Object.Instantiate(tilePrefab, gridPosition, Quaternion.identity);
			return new Tile(gridPosition, selectedTileType, tileObject);
		}

		private bool AddTile(Tile tile) {
			return BoardData.Tiles.Add(tile);
		}
	}
}