using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float Speed;

    private Rigidbody mBody;

    void Awake()
    {
        mBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
        {
            direction = -Vector3.right;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
        }

        if (Input.GetKey(KeyCode.W))
        {
        	if (transform.position.z >= -55)
        	{
				direction += Vector3.up;
        	}
        }
        else if (Input.GetKey(KeyCode.S))
        {
        	if (transform.position.z <= 55)
        	{
				direction += -Vector3.down;
        	}
        }

        mBody.AddForce(direction * Speed * Time.deltaTime);
    }
}
