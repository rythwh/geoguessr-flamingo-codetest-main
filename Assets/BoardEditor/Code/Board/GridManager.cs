using UnityEngine;
using Zenject;

namespace NBoardEditor
{
	public class GridManager
	{
		private const int GridSize = 20;
		public const int GridRadius = GridSize / 2;

		[Inject]
		private GridManager(GameObject grid) {
			grid.transform.position = new Vector3(GridRadius, 0, GridRadius);
			grid.transform.localScale = new Vector3(GridSize, 10, GridSize) / 10f;
		}

		public Vector3Int ConvertToGridPosition(Vector3 position) {
			return new Vector3Int(
				Mathf.RoundToInt(Mathf.Clamp(position.x, 0, GridSize)),
				1,
				Mathf.RoundToInt(Mathf.Clamp(position.z, 0, GridSize))
			);
		}
	}
}