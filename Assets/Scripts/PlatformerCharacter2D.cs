using UnityEngine;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour 
{
	public int p = 1;
	bool facingRight = false;							// For determining which way the player is currently facing.

	[SerializeField] float maxSpeed = 10f;				// The fastest the player can travel in the x axis.
	[SerializeField] float jumpForce = 400f;			// Amount of force added when the player jumps.	
	[SerializeField] float weaponLength = 0.1f;
	[SerializeField] Vector2 weaponOffset;
	[SerializeField] bool airControl = false;			// Whether or not a player can steer while jumping;
	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	
	[System.Serializable]
	public class CharacterTimings {
		
		public float lengthOfAttack;
		
		/// <summary>
		/// Middle of time window before you can attack again
		/// </summary>
		public float timeBetweenAttacks;
		public float timeBetweenAttacksWindowSize;
		[System.NonSerialized] public float lastAttackTime = 0f;
		
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
	bool jump;
	bool attacking = false;
	int combo = 0;
	

    void Awake()
	{
		// Setting up references.
		groundCheck = transform.Find("GroundCheck");
		ceilingCheck = transform.Find("CeilingCheck");
		anim = GetComponent<Animator>();
	}


	void Update()
	{

		
	
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		jump = Input.GetButton("P"+p.ToString()+"Jump");
	
		
		Attack();
		
	}
	
	void FixedUpdate(){
		Move();
	}

	void Move()
	{
		float move = Input.GetAxis("P"+p.ToString()+"Move");
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
            jump = false;
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
	
	
	IEnumerator AttackRoutine() {
		attacking = true;
		timing.lastAttackTime = Time.time;
		Debug.Log ("Attacking! " + timing.lastAttackTime);
		RaycastHit2D hit;
		switch (combo)
		{
		case 1:
			hit = Physics2D.CircleCast(WeaponStart() + weaponOffset, weaponLength, new Vector2(0f,0f));
			if (hit.transform.tag == "Player")
				hit.transform.SendMessage("Damage");
			break;
		case 2:
			break;
		case 3:
			break;
		default:
			combo = 0;
			break;
		}
		
		yield return new WaitForEndOfFrame();
		combo = 0;
		attacking = false;
	}
	
	void Damage() {
		Debug.Log ("I got hit!");
	}
	
	void Attack()
	{
		if (grounded && !attacking) {
			if (Input.GetButtonDown("P"+p.ToString()+"Attack")) {
				Debug.Log ("Attack button");
				if ( combo == 0 ) {
					anim.SetInteger("Combo", ++combo);
					StartCoroutine(AttackRoutine());
				}
				else {
					
					if (Time.time - timing.lastAttackTime > timing.timeBetweenAttacks - timing.timeBetweenAttacksWindowSize/2f &&
						Time.time - timing.lastAttackTime < timing.timeBetweenAttacks + timing.timeBetweenAttacksWindowSize/2f) {
							anim.SetInteger("Combo", ++combo);
							StartCoroutine(AttackRoutine());
					}
					else {
						combo = 0;
						anim.SetInteger("Combo", combo);
					}
				}
			}
		}
	}
	
	Vector2 WeaponStart() {
		return new Vector2(transform.position.x, transform.position.y);
	}
}
