using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	bool facingRight = true;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	

	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
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
	
	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .2f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	Transform ceilingCheck;								// A position marking where to check for ceilings
	float ceilingRadius = .01f;							// Radius of the overlap circle to determine if the player can stand up
	Animator anim;										// Reference to the player's animator component.


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
	
	

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
	}


	void FixedUpdate()
	{

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
	
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	}


	public void Move(float move, bool crouch, bool jump)
	{


		// If crouching, check to see if the character can stand up
		if(!crouch && anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if( Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
				crouch = true;
		}

		// Set whether or not the character is crouching in the animator
		anim.SetBool("Crouch", crouch);

		//only control the player if grounded or airControl is turned on
		if(grounded || airControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			// move = (crouch ? move * crouchSpeed : move);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if(move > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(move < 0 && facingRight)
				// ... flip the player.
				Flip();
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
	}

	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
