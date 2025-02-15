using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NBoardEditor
{
	[Serializable]
	public class Tile
	{
		[SerializeField] private Vector3Int position;
		[SerializeField] private TileTypeEnum tileType;

		public Vector3Int Position => position;
		public TileTypeEnum TileType => tileType;
		public BoardEditorTileObject TileObject { get; private set; }

		public Tile(Vector3Int position, TileTypeEnum tileType, Sprite tileTypeSprite, BoardEditorTileObject tileObject) {
			this.position = position;
			TileObject = tileObject;

			UpdateTileType(tileType, tileTypeSprite);
		}

		public void UpdateTileType(TileTypeEnum tileType, Sprite tileTypeSprite) {
			this.tileType = tileType;
			TileObject.SetImage(tileTypeSprite);
		}

		public void Destroy() {
			Object.Destroy(TileObject.gameObject);
		}
	}
}