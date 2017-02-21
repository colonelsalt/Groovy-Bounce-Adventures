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
	public bool touchingHorizontal, touchingVertical, movedLinear, bounced;
	public int health;
	public GameObject projectile;
	public PowerUp.Type currentPowerType;

    private Rigidbody mBody;
    private Renderer renderer;
	private Vector3 directionForce;
	private PowerUpTimer timerDisplay;
	private float leftBound, rightBound, bottomBound, topBound;
	private HealthCounter healthCounter;
	private bool powerUpActive;
	private float powerUpTimer;

	// TODO: remove this simplified fix for changing colours
	private Color defaultColour;

    void Start()
    {
		leftBound = (-Arena.Width / 2) + VerticalWall.Width;
		rightBound = (Arena.Width / 2) - VerticalWall.Width;
		bottomBound = (-Arena.Height / 2) + HorizontalWall.Height;
		topBound = (Arena.Height / 2) - HorizontalWall.Height;

    	touchingHorizontal = true;
    	touchingVertical = movedLinear = bounced = false;
    	currentPowerType = PowerUp.Type.None;

        mBody = GetComponent<Rigidbody>();
        healthCounter = FindObjectOfType<HealthCounter>();
        timerDisplay = FindObjectOfType<PowerUpTimer>();
        renderer = GetComponent<Renderer>();
        defaultColour = renderer.material.color;
    }

    void Update()
    {
		if (Input.GetButton("Horizontal") && touchingHorizontal)
        {
            transform.position += Input.GetAxisRaw("Horizontal") * Vector3.right * Speed * Time.deltaTime;
			if (transform.position.x != leftBound && transform.position.x != rightBound) touchingVertical = false;
            movedLinear = true;
            bounced = false;
        }
		else if (Input.GetButton("Vertical")  && touchingVertical)
        {
            transform.position += Input.GetAxisRaw("Vertical") * Vector3.forward * Speed * Time.deltaTime;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
            movedLinear = true;
            bounced = false;
        }

        if (Input.GetButton("Bounce") && (touchingVertical || touchingHorizontal))
        {
        	mBody.constraints = RigidbodyConstraints.FreezePositionY;
        	mBody.isKinematic = false;
			Vector3 bounceDirection = GetBounceDirection();
			mBody.velocity = velocityBoost * bounceDirection;
			directionForce = bounceFactor * bounceDirection;
        	bounced = true;
        	movedLinear = false;
        	touchingHorizontal = touchingVertical = false;
        }
        if (Input.GetButtonDown("Fire1") && currentPowerType != PowerUp.Type.None)
        {
        	ActivatePowerUp();
        }
        if (Input.GetButtonUp("Fire1"))
        {
        	DeactivatePowerUp();
        }

        if (bounced) mBody.AddForce(directionForce * Speed * Time.deltaTime);
        if (powerUpActive) CheckPowerUp();

		ClampToPlaySpace();
    }

    void OnCollisionEnter(Collision col)
    {
    	if (col.gameObject.tag == "Enemy")
    	{
    		if (!powerUpActive)
    		{
    			Enemy enemy = col.gameObject.GetComponent<Enemy>();
    			TakeDamage(enemy.Damage);
    		}
    		else if (Input.GetKey(KeyCode.LeftShift) && currentPowerType == PowerUp.Type.Hammer)
    		{
    			Destroy(col.gameObject);
    		}
    	}
    }

	private void FreezePosition()
	{
		mBody.isKinematic = true;
		mBody.constraints = RigidbodyConstraints.FreezeAll;
		mBody.velocity = Vector3.zero;
		bounced = false;
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
		if (transform.position.x < leftBound)
		{
			Debug.Log("Player at left of screen!");
			touchingVertical = true;

			transform.position = new Vector3(leftBound + floatDistance, 0.5f, transform.position.z);
			FreezePosition();
		}
		else if (transform.position.x > rightBound)
		{
			Debug.Log("Player at right of screen!");
			touchingVertical = true;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
			transform.position = new Vector3(rightBound - floatDistance, 0.5f, transform.position.z);
			FreezePosition();
		}
		else if (transform.position.z < bottomBound)
		{
			Debug.Log("Player at bottom of screen!");
			touchingHorizontal = true;
			if (transform.position.z != leftBound && transform.position.z != rightBound) touchingVertical = false;
			transform.position = new Vector3(transform.position.x + floatDistance, 0.5f, bottomBound);
			FreezePosition();
		}
		else if (transform.position.z > topBound)
		{
			Debug.Log("Player at top of screen!");
			touchingHorizontal = true;
			if (transform.position.z != leftBound && transform.position.z != rightBound) touchingVertical = false;
			transform.position = new Vector3(transform.position.x + floatDistance, 0.5f, topBound);
			FreezePosition();
		}
	}

	public void TakeDamage(int amount)
	{
		health -= amount;
		healthCounter.UpdateDisplay();
	}

	public void SetPowerUp(PowerUp.Type type)
	{
		currentPowerType = type;
	}

	private void ActivatePowerUp()
	{
		if (!powerUpActive)
		{
			Debug.Log("Powerup activating!");
			powerUpActive = true;
			powerUpTimer = 20f;
		}
		switch (currentPowerType)
		{
			// TODO: make a neater/flashier graphical effect for these powerups:
			case PowerUp.Type.Hammer:
				renderer.material.color = Color.magenta;
				break;
			case PowerUp.Type.Gun:
				renderer.material.color = Color.cyan;
				InvokeRepeating("FireGun", 0.0000000001f, 0.25f);
				break;
		}
	}

	private void DeactivatePowerUp()
	{
		// TODO: make powerup-deactivation animation neater/flashier
		renderer.material.color = defaultColour;
		if (currentPowerType == PowerUp.Type.Gun)
		{
			CancelInvoke("FireGun");
		}
	}

	private void FireGun()
	{
		Vector3 firePos = new Vector3(transform.position.x, 0.5f, transform.position.z);
		GameObject shot = Instantiate(projectile, firePos, Quaternion.identity) as GameObject;
		Rigidbody fireBody = shot.GetComponent<Rigidbody>();
		if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
		{ // shoot in direction indicated by input direction
			fireBody.velocity = projectileSpeed * ((Input.GetAxis("Vertical") * Vector3.forward)
			+ (Input.GetAxis("Horizontal") * Vector3.right).normalized);
		}
		else
		{ // shoot in the direction of player travel
			fireBody.velocity = mBody.velocity.normalized * projectileSpeed;
		}
	}

	private void CheckPowerUp()
	{
		powerUpTimer -= Time.deltaTime;
		Debug.Log("Powerup timer currently at " + powerUpTimer);
		timerDisplay.UpdateDisplay(powerUpTimer);
		if (powerUpTimer <= 0f)
		{
			powerUpActive = false;
			renderer.material.color = defaultColour;
			currentPowerType = PowerUp.Type.None;
			CancelInvoke("FireGun");
		}
	}
}

