using System;
using UnityEngine;

namespace NGame.Player
{
	public class PlayerProfile
	{
		[SerializeField] private int coins = 0;
		public int Coins => coins;

		public event Action<int, int> OnCoinsAdded;

		public void AddCoins(int amount) {
			coins += amount;
			OnCoinsAdded?.Invoke(amount, coins);
		}
	}
}