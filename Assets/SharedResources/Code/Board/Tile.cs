using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NShared.Board
{
	[Serializable]
	public class Tile
	{
		[SerializeField] private Vector3Int position;
		[SerializeField] private TileTypeEnum tileType;

		public Vector3Int Position => position;
		public TileTypeEnum TileType => tileType;
		public ITileObject TileObject { get; private set; }

		public Tile(Vector3Int position, TileTypeEnum tileType, ITileObject tileObject) {
			this.position = position;
			TileObject = tileObject;
			UpdateTileType(tileType);
		}

		public Tile(Vector3Int position, TileTypeEnum tileType, Sprite tileTypeSprite, ITileObject tileObject) {
			this.position = position;
			TileObject = tileObject;

			UpdateTileType(tileType, tileTypeSprite);
		}

		public void UpdateTileType(TileTypeEnum tileType) {
			this.tileType = tileType;
			// TODO Change the tile colour
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