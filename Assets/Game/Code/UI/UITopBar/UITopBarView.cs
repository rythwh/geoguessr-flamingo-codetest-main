using RyUI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NGame.UI
{
	public class UITopBarView : UIView
	{
		[SerializeField] private TMP_Text rollAmountText;
		[SerializeField] private TMP_Text coinAmountText;

		public void SetCoinAmount(int coins) {
			coinAmountText.SetText(coins.ToString());
			LayoutRebuilder.ForceRebuildLayoutImmediate(coinAmountText.rectTransform);
		}

		public void SetRollAmount(int roll) {
			rollAmountText.SetText(roll.ToString());
		}
	}
}