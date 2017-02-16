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

    	if (col.gameObject.tag == "Horizontal wall") touchingHorizontal = true;
    	else if (col.gameObject.tag == "Vertical wall") touchingVertical = true;
    }
    void OnCollisionExit(Collision col)
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
        }
		else if (Input.GetKey(KeyCode.D) && touchingHorizontal)
        {
            transform.position += Vector3.right * Speed * Time.deltaTime;
        }
		else if (Input.GetKey(KeyCode.W) && touchingVertical)
        {
            transform.position += Vector3.forward * Speed * Time.deltaTime;
        }
		else if (Input.GetKey(KeyCode.S) && touchingVertical)
        {
            transform.position += -Vector3.forward * Speed * Time.deltaTime;
        }
        // contain player within bounds of outer walls
		float newX = Mathf.Clamp(transform.position.x, (-Arena.Width / 2) + VerticalWall.Width, (Arena.Width / 2) - VerticalWall.Width);
		float newZ = Mathf.Clamp(transform.position.z, (-Arena.Height / 2) + HorizontalWall.Height, (Arena.Height / 2) - HorizontalWall.Height);

		transform.position = new Vector3(newX, transform.position.y, newZ);
		mBody.AddForce(directionForce * Speed * Time.deltaTime);
    }
}