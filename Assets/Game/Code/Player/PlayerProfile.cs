using UnityEngine;

namespace NGame.Player
{
	public class PlayerProfile
	{
		[SerializeField] private int coins = 0;
		public int Coins => coins;

		public void AddCoins(int amount) {
			coins += amount;
		}
	}
}