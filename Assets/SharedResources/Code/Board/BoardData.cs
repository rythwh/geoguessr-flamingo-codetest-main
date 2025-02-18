using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NShared.Board
{
	[Serializable]
	public class BoardData
	{
		public static readonly string BoardsFolderPath = Path.Combine(Application.streamingAssetsPath, "Boards");

		[NonSerialized] private HashSet<Tile> tiles = new();
		public HashSet<Tile> Tiles => tiles;

		public List<Tile> OrderedTiles;

		public void RestoreAfterDeserialization() {
			tiles = new HashSet<Tile>(OrderedTiles);
		}

		public Tile GetStartTile() {
			return OrderedTiles.First(t => t.TileType == TileTypeEnum.Start);
		}

		public Tile GetNextTile(Tile currentTile, int distance) {
			int index = OrderedTiles.IndexOf(currentTile);
			int nextIndex = (index + distance) % OrderedTiles.Count;
			return OrderedTiles[nextIndex];
		}

		public List<Tile> GetNextTilePath(Tile startTile, Tile endTile) {
			int startIndex = OrderedTiles.IndexOf(startTile) + 1;
			int endIndex = OrderedTiles.IndexOf(endTile);

			List<Tile> path = new();
			if (endIndex < startIndex) { // Looped around past the Start tile
				for (int i = startIndex; i < OrderedTiles.Count; i++) {
					path.Add(GetPathPoint(i));
				}
				for (int i = 0; i < endIndex + 1; i++) {
					path.Add(GetPathPoint(i));
				}
			} else {
				for (int i = startIndex; i < endIndex + 1; i++) {
					path.Add(GetPathPoint(i));
				}
			}
			return path;
		}

		private Tile GetPathPoint(int index) {

			Tile tile = OrderedTiles[index];
			return tile;
		}
	}
}