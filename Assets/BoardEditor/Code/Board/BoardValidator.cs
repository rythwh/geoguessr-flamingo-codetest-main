using System;
using System.Collections.Generic;
using System.Linq;
using NBoardEditor.UI;
using NShared.Board;
using Zenject;

namespace NBoardEditor
{
	public class BoardValidator : IDisposable
	{
		private readonly PlacementHandler placementHandler;
		private readonly EditorBoardManager editorBoardManager;
		private readonly EditorUIHandler editorUIHandler;

		[Inject]
		public BoardValidator(PlacementHandler placementHandler, EditorBoardManager editorBoardManager, EditorUIHandler editorUIHandler) {
			this.placementHandler = placementHandler;
			this.editorBoardManager = editorBoardManager;
			this.editorUIHandler = editorUIHandler;

			placementHandler.OnBoardUpdated += OnBoardUpdated;
			editorUIHandler.OnBoardLoaded += OnBoardLoaded;
		}

		public void Dispose() {
			placementHandler.OnBoardUpdated -= OnBoardUpdated;
			editorUIHandler.OnBoardLoaded -= OnBoardLoaded;
		}

		private void OnBoardUpdated() {
			ValidateBoard();
		}

		private void OnBoardLoaded(string obj) {
			ValidateBoard();
		}

		public bool ValidateBoard() {
			if (!ValidateStartTile()) {
				editorUIHandler.OnBoardValidationUpdated?.Invoke(false, "Must contain exactly 1 Start tile.");
				return false;
			}
			if (!ValidateLoop()) {
				editorUIHandler.OnBoardValidationUpdated?.Invoke(false, "Board must loop into itself.");
				return false;
			}
			if (!ValidateSinglePath()) {
				editorUIHandler.OnBoardValidationUpdated?.Invoke(false, "Board must not have multiple paths.");
				return false;
			}

			editorBoardManager.DetermineBoardDirection();

			editorUIHandler.OnBoardValidationUpdated?.Invoke(true, "Board validation passed.");
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