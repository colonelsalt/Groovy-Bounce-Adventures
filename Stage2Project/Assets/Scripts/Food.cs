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
		if (col.gameObject.tag == "Horizontal wall" || col.gameObject.tag == "Vertical wall")
		{
			health--;
		}
		if (health <= 0) Destroy(gameObject);
	}

	void OnParticleCollision(GameObject particle)
	{
		if (particle.tag == "Explosion") Destroy(gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Player player = col.gameObject.GetComponent<Player>();
			if (player.currentPowerType == PowerUp.Type.Star && Input.GetButton("Fire1"))
			{ // push food away from player if star powerup used against it
				Vector3 bounceDirection = (player.GetComponent<Rigidbody>().velocity.normalized +
				(Vector3.forward * Random.Range(-0.3f, 0.3f) + Vector3.right * Random.Range(-0.3f, 0.3f))).normalized;
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
	}
}
