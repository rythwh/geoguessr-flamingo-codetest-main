using NShared.Board;
using UnityEngine;
using UnityEngine.UI;

namespace NBoardEditor
{
	public class BoardEditorTileObject : MonoBehaviour, ITileObject
	{
		[SerializeField] private MeshRenderer tileMeshRenderer;
		[SerializeField] private Image tileTypeImage;

		public Renderer Renderer => tileMeshRenderer;
		public Transform TileButtonTransform { get; } = null;

		public void SetImage(Sprite sprite) {
			tileTypeImage.sprite = sprite;
		}

		public void SetColour(Color colour) {
			tileMeshRenderer.material.color = colour;
		}
	}
}