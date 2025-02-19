using RyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI
{
	public class UIQuizView : UIView
	{
		[Header("(Parent) Pre-Answer")]
		[SerializeField] protected TMP_Text titleText;
		[SerializeField] protected TMP_Text questionText;
		[SerializeField] private LayoutGroup answerButtonsLayoutGroup;
		public LayoutGroup AnswerButtonsLayoutGroup => answerButtonsLayoutGroup;

		[Header("(Parent) Post-Answer")]
		[SerializeField] private GameObject postAnswerContainer;
		[SerializeField] private TMP_Text answerResultText;
		[SerializeField] private TMP_Text correctAnswerText;
		[SerializeField] private TMP_Text coinsAmountText;
		[SerializeField] private Button closeButton;
		public Button CloseButton => closeButton;

		public void SetTitle(string title) {
			titleText.SetText(title);
		}

		public void SetQuestion(string question) {
			questionText.SetText(question);
		}

		public void SetPostAnswerActive() {
			postAnswerContainer.SetActive(true);
		}

		public void SetAnswerResultText(string text) {
			answerResultText.SetText(text);
		}

		public void SetCorrectAnswerText(string text) {
			correctAnswerText.SetText(text);
		}

		public void SetCoinsAmountText(int amount) {
			coinsAmountText.SetText(amount.ToString());
		}
	}
}