using NShared.Board;
using UnityEngine;

namespace NGame
{
	public class TileObject : MonoBehaviour, ITileObject
	{
		[SerializeField] private new Renderer renderer;

		public void SetMaterial(Material material) {
			renderer.material = material;
		}
	}
}