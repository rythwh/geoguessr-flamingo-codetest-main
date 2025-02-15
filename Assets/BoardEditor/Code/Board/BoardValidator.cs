using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands.TubeClient;
using NBoardEditor.UI;
using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class BoardValidator
	{
		private readonly PlacementHandler placementHandler;
		private readonly GridManager gridManager;
		private readonly UIHandler uiHandler;

		[Inject]
		public BoardValidator(PlacementHandler placementHandler, GridManager gridManager, UIHandler uiHandler) {
			this.placementHandler = placementHandler;
			this.gridManager = gridManager;
			this.uiHandler = uiHandler;

			placementHandler.OnBoardUpdated += ValidateBoard;
		}

		public void ValidateBoard() {
			if (!ValidateStartTile()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Must contain exactly 1 Start tile.");
				return;
			}
			if (!ValidateLoop()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Board must loop into itself.");
				return;
			}
			if (!ValidateSinglePath()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Board must not have multiple paths.");
				return;
			}

			uiHandler.OnBoardValidationUpdated?.Invoke(true, "Board validation passed.");
		}

		private bool ValidateStartTile() {
			return gridManager.Tiles.Count(t => t.TileType.tileType == TileTypeEnum.Start) == 1;
		}

		private bool ValidateLoop() {

			HashSet<Tile> checkedTiles = new();
			Queue<Tile> frontierTiles = new Queue<Tile>();
			frontierTiles.Enqueue(gridManager.Tiles.FirstOrDefault(t => t.TileType.tileType == TileTypeEnum.Start));

			while (frontierTiles.Count > 0) {
				Tile currentTile = frontierTiles.Dequeue();
				if (!checkedTiles.Add(currentTile)) {
					continue;
				}
				foreach (Tile tile in gridManager.GetSurroundingTilesToTile(currentTile)) {
					if (tile == null) {
						continue;
					}
					frontierTiles.Enqueue(tile);
				}
				if (checkedTiles.Count == gridManager.Tiles.Count) {
					return true;
				}
			}

			return false;
		}

		private bool ValidateSinglePath() {
			bool alwaysTwoNeighbours = true;
			foreach (Tile tile in gridManager.Tiles) {
				alwaysTwoNeighbours = gridManager.GetSurroundingTilesToTile(tile).Count(t => t != null) == 2;
				if (!alwaysTwoNeighbours) {
					break;
				}
			}
			return alwaysTwoNeighbours;
		}


	}
}