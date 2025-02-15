using System.Collections.Generic;
using UnityEngine;

namespace NBoardEditor
{
	[CreateAssetMenu(fileName = "TileTypeList", menuName = "GeoGuessr/Board/TileTypeList", order = 1)]
	public class TileTypeList : ScriptableObject
	{
		public List<TileType> tileTypes;
	}
}