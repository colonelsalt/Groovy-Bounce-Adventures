using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public int ScoreValue;
	public float startForce;
	private Rigidbody mBody;
	private Score score;

	void Awake()
	{
		mBody = GetComponent<Rigidbody>();
		score = FindObjectOfType<Score>();
	}

	void Start ()
	{
		float angle = Random.Range(0, 2 * Mathf.PI);
		mBody.AddForce(new Vector3(startForce * Mathf.Sin(angle), 0, startForce * Mathf.Cos(angle)));
	}

	void Update ()
	{
		//Debug.Log("Food's current velocity is " + mBody.velocity);
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			score.IncrementScore(ScoreValue);
			Destroy(gameObject);
		}
		/*else if (col.gameObject.tag == "Horizontal wall" || col.gameObject.tag == "Vertical wall")
		{
			BounceRandom();
		}*/
	}

	void BounceRandom()
	{
		float angle = Random.Range(0, 2 * Mathf.PI);
		mBody.AddForce(new Vector3(startForce * Mathf.Sin(angle), 0, startForce * Mathf.Cos(angle)));
	}
}
