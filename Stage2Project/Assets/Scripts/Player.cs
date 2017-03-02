using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    public float Speed;
    public float bounceFactor, velocityBoost, floatDistance;
    public float projectileSpeed;
    public float TIME_INVINCIBLE;
	public int health;
	public int MAXHEALTH;
	public GameObject projectilePrefab;
	public GameObject crosshairPrefab;
	public PowerUp.Type currentPowerType;
	public GameObject crashTrailPrefab;
	public GameObject bounceTrailPrefab;
	public GameObject starPrefab;
	public GameObject outerGlowPrefab;
	public GameObject playerDeathPrefab;
	public AudioClip eatingSound, bounceSound, crashSound, damageSound, shieldSound;
	public Material hurtFace;
	public PowerUp.Type[] powerUps;
	public bool isInvincible;

    private Rigidbody mBody;
    private Renderer rend;
    private AudioSource audioSource;
	private PowerUpTimer powerUpTimer;
	private InventoryDisplay inventoryDisplay;
	private float leftBound, rightBound, bottomBound, topBound;
	public bool touchingHorizontal, touchingVertical;
	private float invincibilityTime;
	private HealthCounter healthCounter;
	private Behaviour halo;
	private bool powerUpActive;
	private int numPowerUps;
	public int powerIndex;
	private Vector3 preFreezeVelocity;
	private Quaternion bottomRotation, topRotation, leftRotation, rightRotation;
	private GameObject star;
	private Material defaultMaterial;

    void Start()
    {
		leftBound = (-Arena.Width / 2) + VerticalWall.Width;
		rightBound = (Arena.Width / 2) - VerticalWall.Width;
		bottomBound = (-Arena.Height / 2) + HorizontalWall.Height;
		topBound = (Arena.Height / 2) - HorizontalWall.Height;
    	bottomRotation = Quaternion.Euler(-41, -84, 75);
    	topRotation = Quaternion.Euler(-43, 105, 59);
    	leftRotation = Quaternion.Euler(-43, 16, 59);
    	rightRotation = Quaternion.Euler(-41, 198, 57);
        healthCounter = FindObjectOfType<HealthCounter>();
        powerUpTimer = FindObjectOfType<PowerUpTimer>();
        inventoryDisplay = FindObjectOfType<InventoryDisplay>();
        audioSource = GetComponentInParent<AudioSource>();
		mBody = GetComponent<Rigidbody>();
		rend = GetComponent<Renderer>();
        defaultMaterial = rend.material;
		halo = (Behaviour) GetComponent("Halo");

		Init();
    }

    public void Init()
    {
		health = MAXHEALTH;
		rend.enabled = true;
		GetComponent<Collider>().enabled = true;
		touchingHorizontal = true;
    	touchingVertical = powerUpActive = isInvincible = false;
    	currentPowerType = PowerUp.Type.None;
    	powerUps = new PowerUp.Type[3];
    	numPowerUps = powerIndex = 0;
    	preFreezeVelocity = Vector3.zero;
		transform.rotation = bottomRotation;
		transform.localScale = new Vector3(8, 8, 8);
    }

    void Update()
    {
		if (Input.GetButton("Horizontal"))
        {
			if (touchingHorizontal)
			{ // move linearly
				transform.position += Input.GetAxisRaw("Horizontal") * Vector3.right * Speed * Time.deltaTime;
				if (transform.position.x != leftBound && transform.position.x != rightBound) touchingVertical = false;
			}
			else if (touchingVertical)
			{ // bounce if corresponding key pressed
				if (transform.position.x < 0 && Input.GetAxisRaw("Horizontal") == 1) Bounce();
				else if (transform.position.x > 0 && Input.GetAxisRaw("Horizontal") == -1) Bounce();
			}
        }
		if (Input.GetButton("Vertical"))
        {
        	if (touchingVertical)
        	{ // move linearly
				transform.position += Input.GetAxisRaw("Vertical") * Vector3.forward * Speed * Time.deltaTime;
				if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
        	}
        	else if (touchingHorizontal)
        	{ // bounce if corresponding key pressed
				if (transform.position.z < 0 && Input.GetAxisRaw("Vertical") == 1) Bounce();
				else if (transform.position.z > 0 && Input.GetAxisRaw("Vertical") == -1) Bounce();
        	}	
        }

        if (Input.GetButton("Select1"))
        {
			powerIndex = 0;
			inventoryDisplay.UpdateDisplay();
        }	
        else if (Input.GetButton("Select2") && numPowerUps > 1)
        {
        	powerIndex = 1;
			inventoryDisplay.UpdateDisplay();
		}
        else if (Input.GetButton("Select3") && numPowerUps > 2)
        {
        	powerIndex = 2;
			inventoryDisplay.UpdateDisplay();
		}

		if (Input.GetButtonDown("Fire1") && (powerUps[powerIndex] != PowerUp.Type.None || powerUpActive))
		{
			if (!powerUpActive) StartPowerUp();
			switch (currentPowerType)
			{
				case PowerUp.Type.Star:
					Vector3 starPos = new Vector3(transform.position.x,
											transform.position.y + transform.localScale.y, transform.position.z);
					star = Instantiate(starPrefab, starPos, Quaternion.Euler(90, 0, 0)) as GameObject;
					star.GetComponent<Transform>().SetParent(transform);
					break;
				case PowerUp.Type.Gun:
					preFreezeVelocity = mBody.velocity;
					mBody.velocity /= 4;
					// renderer.material.color = Color.cyan;
					InvokeRepeating("FireGun", 0.0000000001f, 0.15f);
					break;
				case PowerUp.Type.Shield:
					audioSource.PlayOneShot(shieldSound);
					halo.enabled = isInvincible = true;
					break;
			}
		}
		else if (Input.GetButtonUp("Fire1") || !powerUpActive)
		{
			CancelInvoke("FireGun");
			if (preFreezeVelocity.magnitude > 0 && !(touchingHorizontal || touchingVertical))
			{
				mBody.velocity = preFreezeVelocity;
				preFreezeVelocity = Vector3.zero;
			}
			if (star) Destroy(star);
		}

		if (!touchingHorizontal && !touchingVertical) EnsureMinVelocity();

		if (isInvincible && invincibilityTime <= TIME_INVINCIBLE) invincibilityTime += Time.deltaTime;
		else
		{
			rend.sharedMaterial = defaultMaterial;
			invincibilityTime = TIME_INVINCIBLE;
			if (currentPowerType != PowerUp.Type.Shield) isInvincible = false;	
		}
		ClampToPlaySpace();
    }

    private void Bounce()
    {
		audioSource.PlayOneShot(bounceSound);
		mBody.constraints = RigidbodyConstraints.FreezePositionY;
    	mBody.isKinematic = false;
		Vector3 bounceDirection = GetBounceDirection();
		mBody.velocity = velocityBoost * bounceDirection;
		mBody.AddForce(bounceFactor * bounceDirection * Time.deltaTime);
		BounceEffect();
    	touchingHorizontal = touchingVertical = false;
    }

	private void FreezePosition()
	{
		mBody.isKinematic = true;
		mBody.constraints = RigidbodyConstraints.FreezeAll;
		mBody.velocity = Vector3.zero;
	}

	public void ResumeMovement()
	{
		if (preFreezeVelocity.magnitude > 0)
		{
			mBody.constraints = RigidbodyConstraints.FreezePositionY;
			mBody.isKinematic = false;
			mBody.velocity = preFreezeVelocity;
			preFreezeVelocity = Vector3.zero;
		}
		powerUpTimer.CancelTimer();
		powerUpActive = false;
	}

	public void IncreaseSize()
	{
		audioSource.PlayOneShot(eatingSound);
		transform.localScale = transform.localScale * 1.02f;
		Instantiate(outerGlowPrefab, transform);
	}

	private Vector3 GetBounceDirection()
	{
		int playerZ = (transform.position.z > 0) ? 1 : -1; // is player at the top or bottom of the screen?
		int playerX = (transform.position.x > 0) ? 1 : -1; // is player on the right or left side of the screen?

		if (touchingHorizontal && !touchingVertical)
		{
			if (Input.GetButton("Horizontal"))
			{ // bounce diagonally
				return (Input.GetAxisRaw("Horizontal") * Vector3.right * Random.Range(0.6f, 0.8f)
						+ (Vector3.back * playerZ)).normalized;
			}
			else
			{ // bounce straight up
				return ((Vector3.back * playerZ) + (Random.Range(-0.1f, 0.1f) * Vector3.right)).normalized;
			}
		}
		else if (touchingVertical && !touchingHorizontal)
		{
			if (Input.GetButton("Vertical"))
			{ // bounce diagonally
				return (Input.GetAxisRaw("Vertical") * Vector3.forward * Random.Range(0.4f, 0.8f)
						+ (Vector3.left * playerX)).normalized;
			}
			else
			{ // bounce straight right/left
				return ((Vector3.left * playerX) + (Random.Range(-0.1f, 0.1f) * Vector3.forward)).normalized;
			}
		}
		else // i.e. the player is at a corner, touching both a vertical and horizontal wall at the same time
		{
			return ((Vector3.back * playerZ) + (Vector3.left * playerX)).normalized;
		}
	}

	private void ClampToPlaySpace()
	{
		if (transform.position.x - (transform.localScale.x / 2) < leftBound)
		{
			transform.rotation = leftRotation;
			touchingVertical = true;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
			transform.position = new Vector3(leftBound + (transform.localScale.x / 2) + floatDistance, 3f,
								transform.position.z);
			if (mBody.velocity.magnitude > 0) CrashEffect();
			FreezePosition();
		}
		else if (transform.position.x + (transform.localScale.x / 2) > rightBound)
		{
			transform.rotation = rightRotation;
			touchingVertical = true;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
			transform.position = new Vector3(rightBound - (transform.localScale.x / 2) - floatDistance, 3f,
								transform.position.z);
			if (mBody.velocity.magnitude > 0) CrashEffect();
			FreezePosition();
		}
		else if (transform.position.z - (transform.localScale.z / 2) < bottomBound)
		{
			transform.rotation = bottomRotation;
			touchingHorizontal = true;
			if (transform.position.z != leftBound && transform.position.z != rightBound) touchingVertical = false;
			transform.position = new Vector3(transform.position.x, 3f,
								bottomBound + (transform.localScale.z / 2) + floatDistance);
			if (mBody.velocity.magnitude > 0) CrashEffect();
			FreezePosition();
		}
		else if (transform.position.z + (transform.localScale.z / 2) > topBound)
		{
			transform.rotation = topRotation;
			touchingHorizontal = true;
			if (transform.position.z != leftBound && transform.position.z != rightBound) touchingVertical = false;
			transform.position = new Vector3(transform.position.x, 3f,
								topBound - (transform.localScale.z / 2) - floatDistance);
			if (mBody.velocity.magnitude > 0) CrashEffect();
			FreezePosition();
		}
	}

	private void CrashEffect()
	{
		audioSource.PlayOneShot(crashSound);
		Vector3 crashPos = Vector3.zero;
		Quaternion trailRotation = Quaternion.identity;
		if (touchingVertical)
		{
			if (transform.position.x < 0) crashPos = new Vector3(-67.6f, 12f, transform.position.z);
			else crashPos = new Vector3(67.8f, 12f, transform.position.z);
		}
		else if (touchingHorizontal)
		{
			trailRotation = Quaternion.Euler(0f, 90f, 0f);
			if (transform.position.z < 0) crashPos = new Vector3(transform.position.x, 12f, -52.1f);
			else crashPos = new Vector3(transform.position.x, 12f, 50.5f);
		}
		Instantiate(crashTrailPrefab, crashPos, trailRotation);
	}

	private void BounceEffect()
	{
		Vector3 bouncePos = Vector3.zero;
		Quaternion trailRotation = Quaternion.identity;
		if (touchingVertical)
		{
			if (transform.position.x < 0) bouncePos = new Vector3(-67.6f, 12f, transform.position.z);
			else bouncePos = new Vector3(67.8f, 12f, transform.position.z);
		}
		else if (touchingHorizontal)
		{
			trailRotation = Quaternion.Euler(0f, 90f, 0f);
			if (transform.position.z < 0) bouncePos = new Vector3(transform.position.x, 12f, -52.1f);
			else bouncePos = new Vector3(transform.position.x, 12f, 50.5f);
		}
		Instantiate(bounceTrailPrefab, bouncePos, trailRotation);
	}

	private void EnsureMinVelocity()
	{
		if (mBody.velocity.magnitude < 30 && preFreezeVelocity == Vector3.zero) mBody.velocity *= 2;
		else if (mBody.velocity.magnitude <= 0 && preFreezeVelocity == Vector3.zero)
		{
			mBody.velocity = 20 * ((Random.Range(-1f, 1f) * Vector3.forward) +
			(Random.Range(-1f, 1f) * Vector3.right)).normalized;
		}
	}

	public void TakeDamage()
	{
		if (!isInvincible)
		{
			audioSource.PlayOneShot(damageSound);
			rend.sharedMaterial = hurtFace;
			isInvincible = true;
			health--;
			healthCounter.DecrementDisplay();
		}

		invincibilityTime = 0;
		if (health <= 0)
		{
			FreezePosition();
			rend.enabled = false;
			GetComponent<Collider>().enabled = false;
			Instantiate(playerDeathPrefab, transform);
		}
	}

	public void AddPowerUp(PowerUp.Type type)
	{
		if (numPowerUps < 3 && type != PowerUp.Type.ExtraLife) powerUps[numPowerUps++] = type;
		else if (type == PowerUp.Type.ExtraLife && health < MAXHEALTH)
		{
			health++;
			healthCounter.IncrementDisplay();
		}
		inventoryDisplay.UpdateDisplay();
	}

	public void CancelPowerUp()
	{
		powerUpActive = false;
		if (currentPowerType == PowerUp.Type.Shield) halo.enabled = isInvincible = false;
		CancelInvoke("FireGun");
		currentPowerType = PowerUp.Type.None;
	}

	private void StartPowerUp()
	{
		powerUpActive = true;
		RearrangeInventory();
		powerUpTimer.StartTimer();
	}

	private void RearrangeInventory()
	{
		currentPowerType = powerUps[powerIndex];
		powerUps[powerIndex] = PowerUp.Type.None;
		for (int i = 1; i < powerUps.Length; i++)
		{
			if (powerUps[i - 1] == PowerUp.Type.None)
			{
				powerUps[i - 1] = powerUps[i];
				powerUps[i] = PowerUp.Type.None;
			}
		}
		if (--numPowerUps > 0)
		{
			powerIndex = (powerIndex >= numPowerUps) ? powerIndex - 1 : powerIndex;
		}
		inventoryDisplay.UpdateDisplay();
		if (currentPowerType == PowerUp.Type.Bomb) FireBomb();
	}

	private void FireGun()
	{
		Vector3 firePos = new Vector3(transform.position.x, 0.5f, transform.position.z);
		GameObject shot = Instantiate(projectilePrefab, firePos, Quaternion.identity) as GameObject;
		Rigidbody fireBody = shot.GetComponent<Rigidbody>();
		Transform fireTransform = shot.GetComponent<Transform>();
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{ // shoot in direction indicated by input direction
			Vector3 newDirection = ((Input.GetAxis("Vertical") * Vector3.forward) +
									(Input.GetAxis("Horizontal") * Vector3.right).normalized);
			fireTransform.rotation = Quaternion.LookRotation(newDirection);
			fireBody.velocity = projectileSpeed * newDirection;
		}
		else
		{ // shoot in the direction of player travel
			fireTransform.rotation = Quaternion.LookRotation(mBody.velocity);
			fireBody.velocity = mBody.velocity.normalized * projectileSpeed;
		}
	}

	private void FireBomb()
	{
		preFreezeVelocity = mBody.velocity;
		FreezePosition();
		Instantiate(crosshairPrefab);
	}
}

