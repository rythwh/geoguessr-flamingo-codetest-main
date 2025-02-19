using NGame.UI.FlagAnswerButton;
using TMPro;
using UnityEngine;

namespace NGame.UI
{
	public class UIQuizFlagView : UIQuizView
	{
		[Header("(Child) Pre-Answer")]
		[SerializeField] private TMP_Text questionTargetText;

		[Header("(Child) Post-Answer")]
		[SerializeField] private UIQuizFlagAnswerButtonComponent correctFlag;

		public void SetQuestionTarget(string text) {
			questionTargetText.SetText(text);
		}

		public void SetCorrectAnswerFlag(Sprite flag) {
			correctFlag.SetFlagImage(flag);
		}
	}
}