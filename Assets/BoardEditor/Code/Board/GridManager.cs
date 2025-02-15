using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class GridManager
	{
		private const int GridSize = 20;
		public const int GridRadius = GridSize / 2;

		private readonly HashSet<Tile> tiles = new();
		public IReadOnlyList<Tile> Tiles => tiles.ToList();

		[Inject]
		private GridManager(GameObject grid) {
			grid.transform.position = new Vector3(GridRadius, 0, GridRadius);
			grid.transform.localScale = new Vector3(GridSize, 10, GridSize) / 10f;
		}

		public Vector3Int ConvertToGridPosition(Vector3 position) {
			return new Vector3Int(
				Mathf.RoundToInt(Mathf.Clamp(position.x, 0, GridSize)),
				1,
				Mathf.RoundToInt(Mathf.Clamp(position.z, 0, GridSize))
			);
		}

		public bool AddTile(Tile tile) {
			return tiles.Add(tile);
		}

		public bool TryGetTileAtPosition(Vector3Int position, out Tile tile) {
			tile = tiles.FirstOrDefault(t => t.Position == position);
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

			Tile tileToRemove = tiles.FirstOrDefault(t => t.Position == position);
			if (tileToRemove == null) {
				return;
			}

			tileToRemove.Destroy();
			tiles.Remove(tileToRemove);
		}
	}
}