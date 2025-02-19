using RyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI
{
	public class UIQuizTextAnswerButtonComponent : UIElementComponent
	{
		[SerializeField] private TMP_Text answerText;
		[SerializeField] private Button button;
		[SerializeField] private Image buttonImage;

		[SerializeField] private Sprite incorrectAnswerSprite;
		[SerializeField] private Sprite correctAnswerSprite;

		public Button Button => button;

		public void SetAnswerText(string text) {
			answerText.SetText(text);
		}

		public void SetCorrectState(bool correct) {
			buttonImage.sprite = correct ? correctAnswerSprite : incorrectAnswerSprite;
		}
	}
}