using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int Damage;
	public int ScoreValue;

	private Score score;

	void Awake()
	{
		score = FindObjectOfType<Score>();
	}

	void OnParticleCollision(GameObject particle)
	{
		if (particle.tag == "Explosion")
		{
			Destroy(gameObject);
			score.IncrementScore(ScoreValue);
		}
	}

}
