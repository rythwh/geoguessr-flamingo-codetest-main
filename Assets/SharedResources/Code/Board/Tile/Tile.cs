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

		public Tile(Vector3Int position, TileTypeEnum tileType, Material material, ITileObject tileObject) {
			this.position = position;
			TileObject = tileObject;
			UpdateTileType(tileType, material);
		}

		public Tile(Vector3Int position, TileTypeEnum tileType, Sprite tileTypeSprite, ITileObject tileObject) {
			this.position = position;
			TileObject = tileObject;

			UpdateTileType(tileType, tileTypeSprite);
		}

		public void UpdateTileType(TileTypeEnum tileType, Material material) {
			this.tileType = tileType;
			TileObject.SetMaterial(material);
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