using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace NBoardEditor
{
	[Serializable]
	public class BoardManager {

		private readonly TileTypeList tileTypeList;

		public BoardData BoardData { get; private set; } = new();

		public BoardEditorTileObject TilePrefab { get; private set; }

		[Inject]
		public BoardManager(TileTypeList tileTypeList) {
			this.tileTypeList = tileTypeList;
			LoadTilePrefab().Forget();
		}

		public bool AddTile(Tile tile) {
			return BoardData.Tiles.Add(tile);
		}

		public bool TryGetTileAtPosition(Vector3Int position, out Tile tile) {
			tile = BoardData.Tiles.FirstOrDefault(t => t.Position == position);
			return tile != null;
		}

		public List<Tile> GetSurroundingTilesToTile(Tile tile) {
			TryGetTileAtPosition(new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z), out Tile leftTile);
			TryGetTileAtPosition(new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z), out Tile rightTile);
			TryGetTileAtPosition(new Vector3Int(tile.Position.x, tile.Position.y, tile.Position.z - 1), out Tile downTile);
			TryGetTileAtPosition(new Vector3Int(tile.Position.x, tile.Position.y, tile.Position.z + 1), out Tile upTile);

			return new List<Tile> {
				leftTile, rightTile, downTile, upTile
			};
		}

		public void RemovePosition(Vector3Int position) {

			Tile tileToRemove = BoardData.Tiles.FirstOrDefault(t => t.Position == position);
			if (tileToRemove == null) {
				return;
			}

			tileToRemove.Destroy();
			BoardData.Tiles.Remove(tileToRemove);
		}

		public void RecreateBoard(BoardData loadedBoardData) {

			foreach (Tile tile in BoardData.Tiles) {
				tile.Destroy();
			}
			BoardData.Tiles.Clear();

			foreach (Tile tile in loadedBoardData.Tiles) {
				AddTile(CreateTile(tile.Position, tile.TileType));
			}
		}

		private async UniTaskVoid LoadTilePrefab() {
			AsyncOperationHandle<GameObject> tilePrefabHandle = Addressables.LoadAssetAsync<GameObject>("BoardEditor/EditorTile");
			tilePrefabHandle.ReleaseHandleOnCompletion();
			TilePrefab = (await tilePrefabHandle).GetComponent<BoardEditorTileObject>();
		}

		public Tile CreateTile(Vector3Int gridPosition, TileTypeEnum selectedTileType) {

			if (!TilePrefab) {
				Debug.Log("Waiting for tile prefab addressable to load...");
				return null;
			}

			Vector3 placementPosition = gridPosition;
			placementPosition.y = 0.7f;

			BoardEditorTileObject tileObject = Object.Instantiate(TilePrefab, placementPosition, Quaternion.identity);
			return new Tile(gridPosition, selectedTileType, tileTypeList.GetSprite(selectedTileType), tileObject);
		}
	}
}