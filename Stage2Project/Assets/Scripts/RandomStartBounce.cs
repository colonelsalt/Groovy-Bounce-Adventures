using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartBounce : MonoBehaviour {

	public float startForce;
	private Rigidbody mBody;

	void Awake()
	{
		mBody = GetComponent<Rigidbody>();
	}

	void Start ()
	{
		float angle = Random.Range(0, 2 * Mathf.PI);
		mBody.AddForce(new Vector3(startForce * Mathf.Sin(angle), 0, startForce * Mathf.Cos(angle)));
	}

	void BounceRandom()
	{
		float angle = Random.Range(0, 2 * Mathf.PI);
		mBody.AddForce(new Vector3(startForce * Mathf.Sin(angle), 0, startForce * Mathf.Cos(angle)));
	}
}
