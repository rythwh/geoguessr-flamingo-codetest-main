using RyUI;
using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI.FlagAnswerButton
{
	public class UIQuizFlagAnswerButtonComponent : UIElementComponent
	{
		[SerializeField] private Image flagImage;
		[SerializeField] private AspectRatioFitter flagImageAspect;
		[SerializeField] private Button button;
		public Button Button => button;

		[SerializeField] private Image correctStateImage;
		[SerializeField] private Sprite incorrectAnswerSprite;
		[SerializeField] private Sprite correctAnswerSprite;

		public void SetFlagImage(Sprite flag) {
			flagImage.sprite = flag;
			flagImageAspect.aspectRatio = flag.rect.width / flag.rect.height;
		}

		public void SetCorrectState(bool correct) {
			correctStateImage.sprite = correct ? correctAnswerSprite : incorrectAnswerSprite;
			correctStateImage.gameObject.SetActive(true);
		}
	}
}