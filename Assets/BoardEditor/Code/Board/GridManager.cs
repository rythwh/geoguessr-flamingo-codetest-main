using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class GridManager
	{
		private const int GridSize = 20;
		public const int GridRadius = GridSize / 2;

		private readonly Dictionary<Vector3Int, GameObject> tiles = new();

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

		public bool AddTile(Vector3Int position, GameObject tile) {
			return tiles.TryAdd(position, tile);
		}

		public bool ContainsPosition(Vector3Int position) {
			return tiles.ContainsKey(position);
		}

		public void RemovePosition(Vector3Int position) {
			if (!ContainsPosition(position)) {
				return;
			}

			Object.Destroy(tiles[position]);

			tiles.Remove(position);
		}
	}
}