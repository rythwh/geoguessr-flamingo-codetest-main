using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI
{
	public class UIQuizTextView : UIQuizView
	{
		[Header("(Child) Pre-Answer")]
		[SerializeField] private Image image;
		[SerializeField] private AspectRatioFitter imageAspect;

		public void SetImage(Sprite sprite) {
			image.sprite = sprite;
			imageAspect.aspectRatio = sprite.rect.width / sprite.rect.height;
		}


	}
}