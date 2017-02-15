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

    void Awake()
    {
        mBody = GetComponent<Rigidbody>();
		directionForce = -Vector3.forward;
    }

    void OnCollisionStay(Collision col)
    {
    	Debug.Log("Touching " + col.gameObject.tag);

    	if (col.gameObject.tag == "Horizontal wall")
    	{
			directionDistance = Vector3.zero;
			if (Input.GetKey(KeyCode.A))
	        {
	            directionDistance += -Vector3.right * Speed * Time.deltaTime;
	        }
	        else if (Input.GetKey(KeyCode.D))
	        {
	            directionDistance += Vector3.right * Speed * Time.deltaTime;
	        }
    	}
    	else if (col.gameObject.tag == "Vertical wall")
    	{
			directionDistance = Vector3.zero;
			if (Input.GetKey(KeyCode.W))
	        {
	            directionDistance += Vector3.forward * Speed * Time.deltaTime;
	        }
	        else if (Input.GetKey(KeyCode.S))
	        {
	            directionDistance += -Vector3.forward * Speed * Time.deltaTime;
	        }
    	}
		transform.position += directionDistance;
		
    }

    void Update()
    {
    	
		mBody.AddForce(directionForce * Speed * Time.deltaTime);
    }
}