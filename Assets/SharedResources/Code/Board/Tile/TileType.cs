using System;
using System.Collections.Generic;
using NShared.Board;
using UnityEngine;

namespace NBoardEditor
{
	[Serializable]
	public class TileType
	{
		public TileTypeEnum tileType;
		public Sprite tileImage;
		public List<Material> tileMaterials;
	}
}