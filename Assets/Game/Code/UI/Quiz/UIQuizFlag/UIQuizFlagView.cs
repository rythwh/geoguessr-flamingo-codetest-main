using TMPro;
using UnityEngine;

namespace NGame.UI
{
	public class UIQuizFlagView : UIQuizView
	{
		[Header("(Child) Pre-Answer")]
		[SerializeField] private TMP_Text questionTargetText;

		public void SetQuestionTarget(string text) {
			questionTargetText.SetText(text);
		}
	}
}