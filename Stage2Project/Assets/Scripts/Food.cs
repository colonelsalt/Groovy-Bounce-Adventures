using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public int health;
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
			if (player.currentPowerType == PowerUp.Type.Star && Input.GetButton("Fire1"))
			{
				Vector3 bounceDirection = -(col.contacts[0].point - transform.position).normalized;
				mBody.AddForce(bounceDirection * bounceFactor);
				health--;
			}
			else
			{
				player.IncreaseSize();
				score.IncrementScore(ScoreValue);
				Destroy(gameObject);
			}
		}
		else if (col.gameObject.tag == "Horizontal wall" || col.gameObject.tag == "Vertical wall")
		{
			health--;
		}
		if (health <= 0)
		{
			Debug.Log("Food died!");
			Destroy(gameObject);
		}
	}

	void OnParticleCollision(GameObject particle)
	{
		if (particle.tag == "Explosion") Destroy(gameObject);
	}
}
