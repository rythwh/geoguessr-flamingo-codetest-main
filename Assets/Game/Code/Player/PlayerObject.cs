﻿using UnityEngine;

namespace NGame.Player
{
	public class PlayerObject : MonoBehaviour
	{
		[SerializeField] private Animator animator;
		public Animator Animator => animator;

		public readonly int PlayerJumpingAnimation = Animator.StringToHash("PlayerJumping");
		public readonly int PlayerIdleAnimation = Animator.StringToHash("Idle");
	}
}