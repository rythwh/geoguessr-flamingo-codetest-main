using UnityEngine;

namespace NShared.Board
{
	public interface ITileObject
	{
		GameObject gameObject { get; }

		virtual void SetImage(Sprite sprite) {
		}

		virtual void SetMaterial(Material material) {
		}
	}
}