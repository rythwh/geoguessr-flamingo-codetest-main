using System;
using System.Collections.Generic;
using UnityEngine;

namespace NBoardEditor
{
	[Serializable]
	public class BoardData
	{
		[NonSerialized] private HashSet<Tile> tiles = new();
		public HashSet<Tile> Tiles => tiles;

		[SerializeField] private List<Tile> serializedTiles;

		public void PrepareForSerialization() {
			serializedTiles = new List<Tile>(tiles);
		}

		public void RestoreAfterDeserialization() {
			tiles = new HashSet<Tile>(serializedTiles);
		}
	}
}