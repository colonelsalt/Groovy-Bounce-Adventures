﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    public float Speed;
    public float bounceFactor, velocityBoost;

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
		if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && touchingHorizontal)
        {
            transform.position += -Vector3.right * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && touchingHorizontal)
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))  && touchingVertical)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }
		else if ((Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow)) && touchingVertical)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
            movedLinear = true;
            bounced = false;
        }

        else if (Input.GetKey(KeyCode.Space) && (touchingVertical || touchingHorizontal))
        {
        	mBody.constraints = RigidbodyConstraints.None;
			float bounceAngle = GetBounceAngle();
			mBody.velocity = new Vector3(velocityBoost * Mathf.Sin(bounceAngle), 0, velocityBoost * Mathf.Cos(bounceAngle));
			directionForce = new Vector3(bounceFactor * Mathf.Sin(bounceAngle), 0, bounceFactor * Mathf.Cos(bounceAngle));
        	bounced = true;
        	movedLinear = false;
        	touchingHorizontal = touchingVertical = false;
        	Debug.Log("Bounced by " + bounceAngle * Mathf.Rad2Deg);
        }


        if (movedLinear) FreezePosition();
        else if (bounced) mBody.AddForce(directionForce * Speed * Time.deltaTime);

        ClampToCeiling();
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

	private float GetBounceAngle()
	{
		if (touchingHorizontal)
		{
			if (transform.position.z < 0) // if player at the bottom of the screen
			{
				if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				{
					return Random.Range(120 * Mathf.Deg2Rad, 150 * Mathf.Deg2Rad);
				}
				else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				{
					return Random.Range(30 * Mathf.Deg2Rad, 60 * Mathf.Deg2Rad);
				}
			}
			else // player is at the top of the screen
			{
				return 210 * Mathf.Deg2Rad; // 210 deg.
			}
		}
		else if (touchingHorizontal)
		{
			if (transform.position.x > 0) // if player at the right of the screen
			{
				return (-2f * Mathf.PI / 3f); // 120 deg.
			}
			else // player is at the left of the screen
			{
				return (Mathf.PI / 6f); // 30 deg. 
			}
		}
		Debug.Log("DERP!");
		return 0f;
	}

	private void ClampToCeiling()
	{
		Debug.Log("Clamping to " + HorizontalWall.Depth / 2f);
		if (transform.position.y > HorizontalWall.Depth / 2f)
		{
			transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
		}
	}
}

