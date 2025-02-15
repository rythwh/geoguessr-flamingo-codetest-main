using System;
using Cysharp.Threading.Tasks;
using NBoardEditor.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace NBoardEditor
{
	public class PlacementHandler : IDisposable
	{
		private readonly GridManager gridManager;
		private readonly InputHandler inputHandler;

		private BoardEditorTileObject tilePrefab;
		private TileType selectedTileType;

		public event Action OnBoardUpdated;

		[Inject]
		public PlacementHandler(GridManager gridManager, InputHandler inputHandler, UIHandler uiHandler) {
			this.gridManager = gridManager;
			this.inputHandler = inputHandler;

			SetupInputActions();
			LoadTilePrefab().Forget();

			uiHandler.OnTileTypeSelected += OnTileTypeSelected;
		}

		private void OnTileTypeSelected(TileType tileType) {
			selectedTileType = tileType;
		}

		private void SetupInputActions() {
			inputHandler.Actions.Editor.Place.performed += OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed += OnRemovePerformed;

		}

		private async UniTaskVoid LoadTilePrefab() {
			AsyncOperationHandle<GameObject> tilePrefabHandle = Addressables.LoadAssetAsync<GameObject>("BoardEditor/EditorTile");
			tilePrefabHandle.ReleaseHandleOnCompletion();
			tilePrefab = (await tilePrefabHandle).GetComponent<BoardEditorTileObject>();
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

			if (!tilePrefab) {
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

			if (gridManager.TryGetTileAtPosition(gridPosition, out Tile existingTile)) {
				if (existingTile.TileType == selectedTileType) {
					return;
				}

				existingTile.UpdateTileType(selectedTileType);

				OnBoardUpdated?.Invoke();

				return;
			}

			Vector3 placementPosition = gridPosition;
			placementPosition.y = 0.7f;

			BoardEditorTileObject tileObject = Object.Instantiate(tilePrefab, placementPosition, Quaternion.identity);
			Tile tile = new Tile(gridPosition, selectedTileType, tileObject);

			gridManager.AddTile(tile);

			OnBoardUpdated?.Invoke();
		}

		private void OnRemovePerformed(InputAction.CallbackContext context) {
			Vector3Int gridPosition = GetGridPositionClicked();
			gridManager.RemovePosition(gridPosition);

			OnBoardUpdated?.Invoke();
		}

		public void Dispose() {
			inputHandler.Actions.Editor.Place.performed -= OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed -= OnRemovePerformed;

		}
	}
}