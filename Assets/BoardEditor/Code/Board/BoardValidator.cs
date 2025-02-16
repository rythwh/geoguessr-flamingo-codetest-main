using System.Collections.Generic;
using System.Linq;
using NBoardEditor.UI;
using NShared.Board;
using Zenject;

namespace NBoardEditor
{
	public class BoardValidator
	{
		private readonly EditorBoardManager editorBoardManager;
		private readonly UIHandler uiHandler;

		[Inject]
		public BoardValidator(PlacementHandler placementHandler, EditorBoardManager editorBoardManager, UIHandler uiHandler) {
			this.editorBoardManager = editorBoardManager;
			this.uiHandler = uiHandler;

			placementHandler.OnBoardUpdated += OnBoardUpdated;
		}

		private void OnBoardUpdated() {
			ValidateBoard();
		}

		public bool ValidateBoard() {
			if (!ValidateStartTile()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Must contain exactly 1 Start tile.");
				return false;
			}
			if (!ValidateLoop()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Board must loop into itself.");
				return false;
			}
			if (!ValidateSinglePath()) {
				uiHandler.OnBoardValidationUpdated?.Invoke(false, "Board must not have multiple paths.");
				return false;
			}

			uiHandler.OnBoardValidationUpdated?.Invoke(true, "Board validation passed.");
			return true;
		}

		private bool ValidateStartTile() {
			return editorBoardManager.BoardData.Tiles.Count(t => t.TileType == TileTypeEnum.Start) == 1;
		}

		private bool ValidateLoop() {

			HashSet<Tile> checkedTiles = new();
			Queue<Tile> frontierTiles = new Queue<Tile>();
			frontierTiles.Enqueue(editorBoardManager.BoardData.Tiles.FirstOrDefault(t => t.TileType == TileTypeEnum.Start));

			while (frontierTiles.Count > 0) {
				Tile currentTile = frontierTiles.Dequeue();
				if (!checkedTiles.Add(currentTile)) {
					continue;
				}
				foreach (Tile tile in editorBoardManager.GetSurroundingTilesToTile(currentTile)) {
					if (tile == null) {
						continue;
					}
					frontierTiles.Enqueue(tile);
				}
				if (checkedTiles.Count == editorBoardManager.BoardData.Tiles.Count) {
					return true;
				}
			}

			return false;
		}

		private bool ValidateSinglePath() {
			bool alwaysTwoNeighbours = true;
			foreach (Tile tile in editorBoardManager.BoardData.Tiles) {
				alwaysTwoNeighbours = editorBoardManager.GetSurroundingTilesToTile(tile).Count(t => t != null) == 2;
				if (!alwaysTwoNeighbours) {
					break;
				}
			}
			return alwaysTwoNeighbours;
		}


	}
}