using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    public float Speed;
    public float bounceFactor;

    private Rigidbody mBody;
	private Vector3 directionForce;
	private bool touchingHorizontal, touchingVertical, movedLinear, bounced;

    void Awake()
    {
    	touchingHorizontal = touchingVertical = movedLinear = bounced = false;
        mBody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col)
    {
    	Debug.Log("Touching " + col.gameObject.tag);

    	if (col.gameObject.tag == "Horizontal wall")
    	{
    		touchingHorizontal = true;
    		bounced = false;
    		FreezePosition();
		}
    	else if (col.gameObject.tag == "Vertical wall")
    	{
    		touchingVertical = true;
    		bounced = false;
    		FreezePosition();
		}
    }
    void OnTriggerExit(Collider col)
    {
    	Debug.Log("Collision ended!");
    	if (col.gameObject.tag == "Horizontal wall") touchingHorizontal = false;
    	else if (col.gameObject.tag == "Vertical wall") touchingVertical = false;
    }

    void Update()
    {
		if (Input.GetKey(KeyCode.A) && touchingHorizontal)
        {
            transform.position += -Vector3.right * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if (Input.GetKey(KeyCode.D) && touchingHorizontal)
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if (Input.GetKey(KeyCode.W) && touchingVertical)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if (Input.GetKey(KeyCode.S) && touchingVertical)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }

        else if (Input.GetKey(KeyCode.Space) && (touchingVertical || touchingHorizontal))
        {
        	mBody.constraints = RigidbodyConstraints.None;
			if (touchingHorizontal && !touchingVertical)
        	{ // assign bouncing force depending on whether player is at top or bottom of the screen
        		Debug.Log("Bounced vertical");
				if (transform.position.z > 0) // if player at top of screen
				{
					mBody.velocity = new Vector3(bounceFactor * (Mathf.Sqrt(3) / 2f), 0f, 0.5f * bounceFactor);
					directionForce = bounceFactor * Vector3.back;
				}
				else
				{
					mBody.velocity = new Vector3(0.5f * bounceFactor, 0f, bounceFactor * (Mathf.Sqrt(3) / 2f));
					directionForce = bounceFactor * Vector3.back;
				}
        		directionForce = (transform.position.z > 0) ? bounceFactor * Vector3.back : bounceFactor * Vector3.forward;
			} 
			else if (touchingVertical && !touchingHorizontal)
        	{ // assign bouncing force depending on whether player is at the left or at the right side of the screen
        		Debug.Log("Bounced horizontal");
        		directionForce = (transform.position.x > 0) ? bounceFactor * Vector3.left : bounceFactor * Vector3.right; 
        	}
        	bounced = true;
        	movedLinear = false;
        	touchingHorizontal = touchingVertical = false;
        	Debug.Log("Bounced by " + directionForce);
        }


        if (movedLinear) FreezePosition();
        else if (bounced) mBody.AddForce(directionForce * Speed * Time.deltaTime);
    }

	private void FreezePosition()
	{
		Debug.Log("Freezing position!");
		mBody.constraints = RigidbodyConstraints.FreezeAll;
		mBody.velocity = Vector3.zero;

		// contain player within bounds of outer walls
		float newX = Mathf.Clamp(transform.position.x, (-Arena.Width / 2) + VerticalWall.Width, (Arena.Width / 2) - VerticalWall.Width);
		float newZ = Mathf.Clamp(transform.position.z, (-Arena.Height / 2) + HorizontalWall.Height, (Arena.Height / 2) - HorizontalWall.Height);
		transform.position = new Vector3(newX, 0.5f, newZ);
	}

	private float getBounceAngle()
	{
		if (touchingHorizontal && !touchingVertical)
		{
			if (transform.position.z < 0) // if player at the bottom of the screen
			{
				return (Mathf.PI / 6f); // 30 deg. 
			}
			else // player is at the top of the screen
			{
				return (7f * Mathf.PI / 6f); // 210 deg.
			}
		}
		else if (touchingVertical && !touchingHorizontal)
		{
			if (transform.position.x > 0) // if player at the right of the screen
			{
				return (2f * Mathf.PI / 3f); // 120 deg.
			}
			else // player is at the left of the screen
			{
				return (Mathf.PI / 6f); // 30 deg. 
			}
		}
	}
}

