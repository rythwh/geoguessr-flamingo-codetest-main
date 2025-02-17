using System;
using NBoardEditor.UI;
using NShared.Board;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace NBoardEditor
{
	public class PlacementHandler : IDisposable
	{
		private readonly GridManager gridManager;
		private readonly EditorBoardManager editorBoardManager;
		private readonly InputHandler inputHandler;

		private TileType selectedTileType;

		public event Action OnBoardUpdated;

		[Inject]
		public PlacementHandler(GridManager gridManager, EditorBoardManager editorBoardManager, InputHandler inputHandler, EditorUIHandler editorUIHandler) {
			this.gridManager = gridManager;
			this.editorBoardManager = editorBoardManager;
			this.inputHandler = inputHandler;

			SetupInputActions();


			editorUIHandler.OnTileTypeSelected += OnTileTypeSelected;
		}

		private void OnTileTypeSelected(TileType tileType) {
			selectedTileType = tileType;
		}

		private void SetupInputActions() {
			inputHandler.Actions.Editor.Place.performed += OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed += OnRemovePerformed;

		}

		private Vector3Int GetGridPositionClicked() {

			if (EventSystem.current.IsPointerOverGameObject()) {
				return default;
			}

			Vector2 mousePosition = Mouse.current.position.ReadValue();

			if (!Camera.main) {
				return default;
			}

			Ray ray = Camera.main.ScreenPointToRay(mousePosition);

			Vector3Int gridPosition = default;
			if (Physics.Raycast(ray, out RaycastHit hit)) {
				gridPosition = gridManager.ConvertToGridPosition(hit.point);
			}

			if (gridPosition == default(Vector3Int)) {
				return default;
			}

			return gridPosition;
		}

		private void OnPlacePerformed(InputAction.CallbackContext context) {

			if (selectedTileType == null) {
				Debug.LogWarning("Select a tile type first.");
				return;
			}

			Vector3Int gridPosition = GetGridPositionClicked();
			if (gridPosition == default(Vector3Int)) {
				return;
			}

			if (editorBoardManager.TryGetTileAtPosition(gridPosition, out Tile existingTile)) {
				if (existingTile.TileType == selectedTileType.tileType) {
					return;
				}

				existingTile.UpdateTileType(selectedTileType.tileType, selectedTileType.tileImage);

				OnBoardUpdated?.Invoke();

				return;
			}

			Tile tile = editorBoardManager.CreateTile(gridPosition, selectedTileType.tileType);

			editorBoardManager.AddTile(tile);

			OnBoardUpdated?.Invoke();
		}

		private void OnRemovePerformed(InputAction.CallbackContext context) {
			Vector3Int gridPosition = GetGridPositionClicked();
			editorBoardManager.RemovePosition(gridPosition);

			OnBoardUpdated?.Invoke();
		}

		public void Dispose() {
			inputHandler.Actions.Editor.Place.performed -= OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed -= OnRemovePerformed;

		}
	}
}