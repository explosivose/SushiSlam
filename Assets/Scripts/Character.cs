using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	
	public string player = "P1";
	
	[System.Serializable]
	public class CharacterTimings {
		
		/// <summary>
		/// Middle of time window before you can attack again
		/// </summary>
		public float timeBetweenAttacks;
		public float timeBetweenAttacksWindowSize;
		
		/// <summary>
		/// The cooldown after attack3.
		/// </summary>
		public float cooldownAfter3Attacks;
		
		/// <summary>
		/// Middle of time window that permits a mid-air recovery
		/// </summary>
		public float recoveryAfterDamagedJump;
		public float recoveryAfterDamagedJumpWindowSize;
	}
	public CharacterTimings timing;
	
	public enum CharacterState {
		idle, 
		running,
		blocking,
		jumping,
		attack1,
		attack2,
		attack3,
		attackJump,
		attackBlocked,
		damaged,
		damagedJump,
	}
	private CharacterState state;
	
	private PlatformerCharacter2D platformer;
	private bool jump;
	
	// Use this for initialization
	void Start () {
		platformer = GetComponent<PlatformerCharacter2D>();
	}
	
	void FixedUpdate() {
		switch (state) {
		case CharacterState.idle:
			break;
		case CharacterState.running:
			break;
		case CharacterState.blocking:
			break;
		case CharacterState.jumping:
			break;
		case CharacterState.attack1:
			break;
		case CharacterState.attack2:
			break;
		case CharacterState.attack3:
			break;
		case CharacterState.attackJump:
			break;
		case CharacterState.attackBlocked:
			break;
		case CharacterState.damaged:
			break;
		case CharacterState.damagedJump:
			break;
		}
	}
	
	void IdleUpdate() {
		if (Input.GetAxis(player+"Move") != 0) {
			state = CharacterState.running;
		}
	}
	
	void RunUpdate() {
		float input = Input.GetAxis(player+"Move");
		platformer.Move (input, false, jump);
		jump = false;
	}
	
	void Jump() {
		/// get jump input and JUMP
		if (Input.GetButtonDown(player+"Jump")) {
			state = CharacterState.jumping;
		} 
	}
}
