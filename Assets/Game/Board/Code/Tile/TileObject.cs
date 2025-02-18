using NShared.Board;
using UnityEngine;

namespace NGame
{
	public class TileObject : MonoBehaviour, ITileObject
	{
		[SerializeField] private new Renderer renderer;
		public Renderer Renderer => renderer;

		[SerializeField] private Transform tileButtonTransform;
		public Transform TileButtonTransform => tileButtonTransform;

		public void SetMaterial(Material material) {
			renderer.material = material;
		}
	}
}