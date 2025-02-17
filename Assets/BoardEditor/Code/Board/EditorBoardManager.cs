using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NShared;
using NShared.Board;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace NBoardEditor
{
	[Serializable]
	public class EditorBoardManager {

		private readonly TileTypeList tileTypeList;

		public BoardData BoardData { get; private set; } = new();

		public BoardEditorTileObject TilePrefab { get; private set; }

		[Inject]
		public EditorBoardManager(TileTypeList tileTypeList) {
			this.tileTypeList = tileTypeList;
			LoadTilePrefab().Forget();
		}

		private async UniTaskVoid LoadTilePrefab() {
			TilePrefab = await Addressable.Load<BoardEditorTileObject>("BoardEditor/EditorTile");
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

		public void DetermineBoardDirection() {

			Tile startTile = BoardData.GetStartTile();

			HashSet<Tile> checkedTiles = new() { startTile }; // For quick Contains checks (but doesn't maintain order)
			List<Tile> orderedLoop = new() { startTile };
			Queue<Tile> frontierTiles = new();

			frontierTiles.Enqueue(GetSurroundingTilesToTile(startTile).First(t => t != null));
			while (frontierTiles.Count > 0) {
				Tile currentTile = frontierTiles.Dequeue();
				if (checkedTiles.Contains(currentTile)) {
					break;
				}

				orderedLoop.Add(currentTile);
				checkedTiles.Add(currentTile);

				Tile nextTile = GetSurroundingTilesToTile(currentTile).FirstOrDefault(t => t != null && !checkedTiles.Contains(t));
				if (nextTile == null) {
					break;
				}
				frontierTiles.Enqueue(nextTile);
			}

			if (CalculateShoelaceFormulaSignedArea(orderedLoop) > 0) {
				orderedLoop.Reverse();
			}

			BoardData.OrderedTiles = orderedLoop;
		}

		// https://en.wikipedia.org/wiki/Shoelace_formula
		// Takes an ordered loop (i.e. shape of points) and determines the "signed area", which is positive
		// if the loop is counterclockwise, and negative if the loop is clockwise
		// Used to ensure that the board is ordered clockwise so next-tile calculations can be simplified
		private int CalculateShoelaceFormulaSignedArea(List<Tile> orderedLoop) {
			int area = 0;
			for (int i = 0; i < orderedLoop.Count; i++) {
				Tile currentTile = orderedLoop[i];
				Tile nextTile = orderedLoop[(i + 1) % orderedLoop.Count];

				area += (currentTile.Position.x * nextTile.Position.y) - (nextTile.Position.x * currentTile.Position.y);
			}

			return area;
		}
	}
}