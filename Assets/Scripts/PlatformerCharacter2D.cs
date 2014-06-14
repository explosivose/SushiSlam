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
	[SerializeField] float knockbackForce = 1000f;
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
		
		public float lengthOfBlock;
		public float blockCooldown;
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
	bool blocked = false;
	bool canBlock = true;
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
		if (grounded) anim.SetBool("Jump", false);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
		jump = Input.GetButton("P"+p.ToString()+"Jump");
	
		
		Attack();
		Block();
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
            anim.SetBool("Jump", true);
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
		RaycastHit2D hit;
		Vector2 direction = Vector2.right;
		if (!facingRight) direction = -Vector2.right;
		switch (combo)
		{
		case 1:
			Debug.DrawRay(transform.position, direction * weaponLength, Color.red, 1f);
			hit = Physics2D.CircleCast(WeaponStart() + weaponOffset, weaponLength, direction);
			if (hit.transform.tag == "Player" && hit.transform != transform) {
				Debug.Log ("I hit a player");
				hit.transform.SendMessage("Damage");
			}
				
			break;
		case 2:
			Debug.DrawRay(transform.position, direction * weaponLength, Color.red, 1f);
			hit = Physics2D.CircleCast(WeaponStart() + weaponOffset, weaponLength, direction);
			if (hit.transform.tag == "Player" && hit.transform != transform) {
				Debug.Log ("I hit a player");
				hit.transform.SendMessage("Damage");
			}
			break;
		case 3:
			Debug.DrawRay(transform.position, direction * weaponLength, Color.red, 1f);
			hit = Physics2D.CircleCast(WeaponStart() + weaponOffset, weaponLength, direction);
			if (hit.transform.tag == "Player" && hit.transform != transform) {
				Debug.Log ("I knocked a player");
				hit.rigidbody.AddForce((direction+Vector2.up) * knockbackForce);
				hit.transform.SendMessage("Knockback");
			}
			break;
		default:
			combo = 0;
			break;
		}
		
		yield return new WaitForEndOfFrame();
		attacking = false;
	}
	
	public IEnumerator Damage() {
		if (blocked) {
			Debug.Log ("I blocked");
		}
		else {
			Debug.Log ("I got hit!");
			if (!grounded) {
				yield return StartCoroutine(Knockback());
			}
			else {
				anim.SetBool("Damaged", true);
				yield return new WaitForSeconds(1f);
				anim.SetBool("Damaged", false);
			}
		}

	}
	
	public IEnumerator Knockback() {
		anim.SetBool("Damaged", true);
		airControl = false;
		yield return new WaitForSeconds(1f);
		airControl = true;
		anim.SetBool("Damaged", false);
	}
	
	void Block() {
		if (Input.GetButtonDown("P"+p.ToString()+"Block") && canBlock && !attacking) {
			Debug.Log ("Blocking!");
			StartCoroutine(BlockRoutine());
		}
	}
	
	IEnumerator BlockRoutine() {
		blocked = true;
		canBlock = false;
		anim.SetBool("Block", blocked);
		yield return new WaitForSeconds(timing.lengthOfBlock);
		blocked = false;
		anim.SetBool("Block", blocked);
		yield return new WaitForSeconds(timing.blockCooldown);
		canBlock = true;
	}
	
	void Attack()
	{
		if (grounded && !attacking) {
			if (Input.GetButtonDown("P"+p.ToString()+"Attack")) {
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
		// waited too long, combo back to zero
		if (Time.time > timing.lastAttackTime + timing.timeBetweenAttacks + timing.timeBetweenAttacksWindowSize) {
			combo = 0;
			anim.SetInteger("Combo", combo);
		}
	}
	
	Vector2 WeaponStart() {
		return new Vector2(transform.position.x, transform.position.y);
	}
}
