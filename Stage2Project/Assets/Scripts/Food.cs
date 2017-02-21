using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public int ScoreValue;
	public float bounceFactor;

	private Score score;
	private Rigidbody mBody;

	void Awake()
	{
		score = FindObjectOfType<Score>();
		mBody = GetComponent<Rigidbody>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			Player player = col.gameObject.GetComponent<Player>();
			if (player.currentPowerType == PowerUp.Type.Hammer && Input.GetButton("Fire1"))
			{
				Vector3 bounceDirection = -(col.contacts[0].point - transform.position).normalized;
				mBody.AddForce(bounceDirection * bounceFactor);
			}
			else
			{
				score.IncrementScore(ScoreValue);
				Destroy(gameObject);
			}
		}
	}
}
