using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    public float Speed;

    private Rigidbody mBody;
	private Vector3 directionForce;
	private Vector3 directionDistance;
	private bool touchingHorizontal, touchingVertical;

    void Awake()
    {
    	touchingHorizontal = touchingVertical = false;
        mBody = GetComponent<Rigidbody>();
		directionForce = -Vector3.forward;
    }

    void OnCollisionEnter(Collision col)
    {
    	Debug.Log("Touching " + col.gameObject.tag);

    	if (col.gameObject.tag == "Horizontal wall")
    	{
    		touchingHorizontal = true;
    		touchingVertical = false;
		}
    	else if (col.gameObject.tag == "Vertical wall")
    	{
    		touchingVertical = true;
    		touchingHorizontal = false;
		}
    }
    void OnCollisionExit(Collision col)
    {
    	Debug.Log("Collision ended!");
    	if (col.gameObject.tag == "Horizontal wall") touchingHorizontal = false;
    	else if (col.gameObject.tag == "Vertical wall") touchingVertical = false;
    }

    void Update()
    {
		directionDistance = Vector3.zero;
		if (Input.GetKey(KeyCode.A) && touchingHorizontal)
        {
            directionDistance += -Vector3.right * Speed * Time.deltaTime;
        }
		else if (Input.GetKey(KeyCode.D) && touchingHorizontal)
        {
            directionDistance += Vector3.right * Speed * Time.deltaTime;
        }
		else if (Input.GetKey(KeyCode.W) && touchingVertical)
        {
            directionDistance += Vector3.forward * Speed * Time.deltaTime;
        }
		else if (Input.GetKey(KeyCode.S) && touchingVertical)
        {
            directionDistance += -Vector3.forward * Speed * Time.deltaTime;
        }

		transform.position += directionDistance;
		mBody.AddForce(directionForce * Speed * Time.deltaTime);
    }
}