using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    public float Speed;
    public float bounceFactor, velocityBoost, floatDistance;
	public bool touchingHorizontal, touchingVertical, movedLinear, bounced;
	public int health;

    private Rigidbody mBody;
	private Vector3 directionForce;
	private float leftBound, rightBound, bottomBound, topBound;
	private HealthCounter healthCounter;


    void Start()
    {
    	touchingHorizontal = true;
    	touchingVertical = movedLinear = bounced = false;
		leftBound = (-Arena.Width / 2) + VerticalWall.Width;
		rightBound = (Arena.Width / 2) - VerticalWall.Width;
		bottomBound = (-Arena.Height / 2) + HorizontalWall.Height;
		topBound = (Arena.Height / 2) - HorizontalWall.Height;
        mBody = GetComponent<Rigidbody>();
        healthCounter = FindObjectOfType<HealthCounter>();
    }

   /* void OnTriggerEnter(Collider col)
    {
    	Debug.Log("Touching " + col.gameObject.tag);

    	if (col.gameObject.tag == "Horizontal wall")
    	{
    		touchingHorizontal = true;
    		FreezePosition();
		}
    	else if (col.gameObject.tag == "Vertical wall")
    	{
    		touchingVertical = true;
    		FreezePosition();
		}
    }
    void OnTriggerExit(Collider col)
    {
    	Debug.Log("Collision ended!");
    	if (col.gameObject.tag == "Horizontal wall") touchingHorizontal = false;
    	else if (col.gameObject.tag == "Vertical wall") touchingVertical = false;
    }*/

    void Update()
    {
		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && touchingHorizontal)
        {
            transform.position += -Vector3.right * Speed * Time.deltaTime;
			if (transform.position.x != leftBound && transform.position.x != rightBound) touchingVertical = false;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && touchingHorizontal)
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
			if (transform.position.x != leftBound && transform.position.x != rightBound) touchingVertical = false;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))  && touchingVertical)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && touchingVertical)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
			if (transform.position.z != bottomBound && transform.position.z != topBound) touchingHorizontal = false;
            movedLinear = true;
            bounced = false;
        }

        if (Input.GetKey(KeyCode.Space) && (touchingVertical || touchingHorizontal))
        {
        	mBody.constraints = RigidbodyConstraints.FreezePositionY;
        	mBody.isKinematic = false;
			float bounceAngle = GetBounceAngle();
			mBody.velocity = new Vector3(velocityBoost * Mathf.Sin(bounceAngle), 0, velocityBoost * Mathf.Cos(bounceAngle));
			directionForce = new Vector3(bounceFactor * Mathf.Sin(bounceAngle), 0, bounceFactor * Mathf.Cos(bounceAngle));
        	bounced = true;
        	movedLinear = false;
        	touchingHorizontal = touchingVertical = false;
        	Debug.Log("Bounced by " + bounceAngle * Mathf.Rad2Deg);
        }
        else if (bounced) mBody.AddForce(directionForce * Speed * Time.deltaTime);

		ClampToPlaySpace();
    }

	private void FreezePosition()
	{
		//Debug.Log("Freezing position!");
		mBody.isKinematic = true;
		mBody.constraints = RigidbodyConstraints.FreezeAll;
		mBody.velocity = Vector3.zero;
		bounced = false;

		// contain player within bounds of outer walls
		/*float newX = Mathf.Clamp(transform.position.x, (-Arena.Width / 2) + VerticalWall.Width, (Arena.Width / 2) - VerticalWall.Width);
		float newZ = Mathf.Clamp(transform.position.z, (-Arena.Height / 2) + HorizontalWall.Height, (Arena.Height / 2) - HorizontalWall.Height);
		transform.position = new Vector3(newX, 0.5f, newZ);*/
	}

	private float GetBounceAngle()
	{
		if (touchingHorizontal && !touchingVertical)
		{
			if (transform.position.z < 0) // if player at the bottom of the screen
			{
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					return Random.Range(-60 * Mathf.Deg2Rad, -30 * Mathf.Deg2Rad);
				}
				else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					return Random.Range(30 * Mathf.Deg2Rad, 60 * Mathf.Deg2Rad);
				}
				else
				{
					return Random.Range(-10f * Mathf.Deg2Rad, 10 * Mathf.Deg2Rad);
				}
			}
			else // player is at the top of the screen
			{
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				{
					return Random.Range(210 * Mathf.Deg2Rad, 240 * Mathf.Deg2Rad);
				}
				else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				{
					return Random.Range(120 * Mathf.Deg2Rad, 150 * Mathf.Deg2Rad);
				}
				else
				{
					return Random.Range(170 * Mathf.Deg2Rad, 190 * Mathf.Deg2Rad);
				}
			}
		}
		else if (touchingVertical && !touchingHorizontal)
		{
			if (transform.position.x > 0) // if player at the right of the screen
			{
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					return Random.Range(-60 * Mathf.Deg2Rad, -30 * Mathf.Deg2Rad);
				}
				else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					return Random.Range(-120 * Mathf.Deg2Rad, -150 * Mathf.Deg2Rad);
				}
				else
				{
					return Random.Range(-100 * Mathf.Deg2Rad, -80 * Mathf.Deg2Rad);
				}
			}
			else // player is at the left of the screen
			{
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				{
					return Random.Range(30 * Mathf.Deg2Rad, 60 * Mathf.Deg2Rad);
				}
				else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				{
					return Random.Range(120 * Mathf.Deg2Rad, 150 * Mathf.Deg2Rad);
				}
				else 
				{
					return Random.Range(80 * Mathf.Deg2Rad, 100 * Mathf.Deg2Rad); 
				}				
			}
		}
		else // i.e. the player is at a corner, touching both a vertical and horizontal wall at the same time
		{
			if (transform.position.x < 0)
			{
				if (transform.position.z < 0) // player at bottom left
				{
					return 30 * Mathf.Deg2Rad;
				}
				else // player at top left
				{
					return 120 * Mathf.Deg2Rad;
				}
			}
			else
			{
				if (transform.position.z < 0) // player at bottom right
				{
					return -30 * Mathf.Deg2Rad;
				}
				else // player at top right
				{
					return -120 * Mathf.Deg2Rad;
				}
			}
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
}

