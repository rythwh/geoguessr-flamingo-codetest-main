using System.Collections.Generic;
using NBoardEditor;
using NShared.Board;
using UnityEngine;

namespace NShared
{
	[CreateAssetMenu(fileName = "TileTypeList", menuName = "GeoGuessr/Board/TileTypeList", order = 1)]
	public class TileTypeList : ScriptableObject
	{
		public List<TileType> tileTypes;
		private int previousMaterialIndex = -1;

		public TileType GetTileType(TileTypeEnum tileType) {
			return tileTypes.Find(t => t.tileType == tileType);
		}

		public Sprite GetSprite(TileTypeEnum tileType) {
			return GetTileType(tileType).tileImage;
		}

		public Material GetMaterial(TileTypeEnum tileTypeEnum) {
			TileType tileType = GetTileType(tileTypeEnum);
			int index = Random.Range(0, tileType.tileMaterials.Count);
			if (tileType.tileMaterials.Count > 1) {
				if (previousMaterialIndex == -1 || index == previousMaterialIndex) {
					index = (index + 1) % tileType.tileMaterials.Count;
				}
			}
			previousMaterialIndex = index;
			return tileType.tileMaterials[index];
		}
	}
}