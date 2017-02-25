using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int Damage;
	public int ScoreValue;

	private Score score;
	private Player player;

	void Awake()
	{
		score = FindObjectOfType<Score>();
		player = FindObjectOfType<Player>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player" && player.currentPowerType != PowerUp.Type.Shield)
		{
			if (player.currentPowerType == PowerUp.Type.Hammer && Input.GetButton("Fire1"))
			{
				score.IncrementScore(ScoreValue);
				Destroy(gameObject);
			}
			else
			{
				player.TakeDamage(Damage);
			}
		}
		else if (col.gameObject.tag == "Projectile")
		{
			score.IncrementScore(ScoreValue);
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
	}

	void OnParticleCollision(GameObject particle)
	{
		if (particle.tag == "Explosion")
		{
			score.IncrementScore(ScoreValue);
			Destroy(gameObject);
		}
	}
}
