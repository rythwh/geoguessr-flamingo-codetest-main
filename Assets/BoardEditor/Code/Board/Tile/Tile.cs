using UnityEngine;

namespace NBoardEditor
{
	public class Tile
	{
		public Vector3Int Position { get; }
		public TileType TileType { get; private set; }
		public BoardEditorTileObject TileObject { get; private set; }

		public Tile(Vector3Int position, TileType tileType, BoardEditorTileObject tileObject) {
			Position = position;
			TileType = tileType;
			TileObject = tileObject;
			tileObject.SetImage(tileType.tileImage);
		}

		public void UpdateTileType(TileType tileType) {
			TileType = tileType;
			TileObject.SetImage(tileType.tileImage);
		}

		public void Destroy() {
			Object.Destroy(TileObject.gameObject);
		}
	}
}