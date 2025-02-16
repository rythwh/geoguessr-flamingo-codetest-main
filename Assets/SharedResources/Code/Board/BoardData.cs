using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NShared.Board
{
	[Serializable]
	public class BoardData
	{
		public static readonly string BoardsFolderPath = Path.Combine(Application.streamingAssetsPath, "Boards");

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