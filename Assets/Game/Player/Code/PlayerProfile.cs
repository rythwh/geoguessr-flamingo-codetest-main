using UnityEngine;

namespace NPlayer
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