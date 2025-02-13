using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
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

		private GameObject tilePrefab;

		[Inject]
		public PlacementHandler(GridManager gridManager, InputHandler inputHandler) {
			this.gridManager = gridManager;
			this.inputHandler = inputHandler;

			SetupInputActions();
			LoadTilePrefab().Forget();
		}

		private void SetupInputActions() {
			inputHandler.Actions.Editor.Place.performed += OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed += OnRemovePerformed;
			inputHandler.Actions.Editor.Zoom.performed += OnZoomPerformed;
		}

		private async UniTaskVoid LoadTilePrefab() {
			AsyncOperationHandle<GameObject> tilePrefabHandle = Addressables.LoadAssetAsync<GameObject>("Board/Tile");
			tilePrefabHandle.ReleaseHandleOnCompletion();
			tilePrefab = await tilePrefabHandle;
		}

		private Vector3Int GetGridPositionClicked() {

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

			Vector3Int gridPosition = GetGridPositionClicked();

			if (gridManager.ContainsPosition(gridPosition)) {
				return;
			}

			Vector3 placementPosition = gridPosition;
			placementPosition.y = 0.7f;

			GameObject tile = Object.Instantiate(tilePrefab, placementPosition, Quaternion.identity);

			gridManager.AddTile(gridPosition, tile);
		}

		private void OnRemovePerformed(InputAction.CallbackContext context) {
			Vector3Int gridPosition = GetGridPositionClicked();
			gridManager.RemovePosition(gridPosition);
		}

		private void OnZoomPerformed(InputAction.CallbackContext context) {
			if (!Camera.main) {
				return;
			}

			Camera.main.orthographicSize += context.ReadValue<float>();
		}

		public void Dispose() {
			inputHandler.Actions.Editor.Place.performed -= OnPlacePerformed;
			inputHandler.Actions.Editor.Remove.performed -= OnRemovePerformed;
			inputHandler.Actions.Editor.Zoom.performed -= OnZoomPerformed;
		}
	}
}