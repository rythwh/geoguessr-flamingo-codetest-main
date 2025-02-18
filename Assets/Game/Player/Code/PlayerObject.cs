using UnityEngine;

namespace NPlayer
{
	public class PlayerObject : MonoBehaviour
	{
		[SerializeField] private Animator animator;
		public Animator Animator => animator;

		public readonly int PlayerJumpingAnimation = Animator.StringToHash("PlayerJumping");
	}
}