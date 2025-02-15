using System;
using NBoardEditor.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

namespace NBoardEditor
{
	public class PlacementHandler : IDisposable
	{
		private readonly GridManager gridManager;
		private readonly BoardManager boardManager;
		private readonly InputHandler inputHandler;

		private TileType selectedTileType;

		public event Action OnBoardUpdated;

		[Inject]
		public PlacementHandler(GridManager gridManager, BoardManager boardManager, InputHandler inputHandler, UIHandler uiHandler) {
			this.gridManager = gridManager;
			this.boardManager = boardManager;
			this.inputHandler = inputHandler;

			SetupInputActions();


			uiHandler.OnTileTypeSelected += OnTileTypeSelected;
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

			if (!boardManager.TilePrefab) {
				Debug.Log("Waiting for tile prefab addressable to load...");
				return;
			}

			if (selectedTileType == null) {
				Debug.LogWarning("Select a tile type first.");
				return;
			}

			Vector3Int gridPosition = GetGridPositionClicked();
			if (gridPosition == default(Vector3Int)) {
				return;
			}

			if (boardManager.TryGetTileAtPosition(gridPosition, out Tile existingTile)) {
				if (existingTile.TileType == selectedTileType.tileType) {
					return;
				}

				existingTile.UpdateTileType(selectedTileType.tileType, selectedTileType.tileImage);

				OnBoardUpdated?.Invoke();

				return;
			}

			Tile tile = boardManager.CreateTile(gridPosition, selectedTileType.tileType);

			boardManager.AddTile(tile);

			OnBoardUpdated?.Invoke();
		}

		private void OnRemovePerformed(InputAction.CallbackContext context) {
			Vector3Int gridPosition = GetGridPositionClicked();
			boardManager.RemovePosition(gridPosition);

			OnBoardUpdated?.Invoke();
		}

		public void Dispose() {
			inputHandler.Actions.Editor.Place.performed -= OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed -= OnRemovePerformed;

		}
	}
}