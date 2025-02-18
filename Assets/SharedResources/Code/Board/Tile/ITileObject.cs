using UnityEngine;

namespace NShared.Board
{
	public interface ITileObject
	{
		GameObject gameObject { get; }
		Renderer Renderer { get; }
		Transform TileButtonTransform { get; }

		virtual void SetImage(Sprite sprite) {
		}

		virtual void SetMaterial(Material material) {
		}

		virtual void SetColour(Color colour) {
		}
	}
}