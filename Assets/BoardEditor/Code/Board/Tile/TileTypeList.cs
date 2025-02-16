using System.Collections.Generic;
using NShared.Board;
using UnityEngine;

namespace NBoardEditor
{
	[CreateAssetMenu(fileName = "TileTypeList", menuName = "GeoGuessr/Board/TileTypeList", order = 1)]
	public class TileTypeList : ScriptableObject
	{
		public List<TileType> tileTypes;

		public Sprite GetSprite(TileTypeEnum tileType) {
			return tileTypes.Find(t => t.tileType == tileType).tileImage;
		}
	}
}