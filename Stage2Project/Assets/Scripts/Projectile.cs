using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	private Score score;

	void Awake()
	{
		score = FindObjectOfType<Score>();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Horizontal wall" || col.gameObject.tag == "Vertical wall")
		{
			Destroy(gameObject);
		}
		else if (col.gameObject.tag == "Enemy")
		{
			Enemy enemy = col.gameObject.GetComponent<Enemy>();
			score.IncrementScore(enemy.ScoreValue);
			Destroy(col.gameObject);
			Destroy(gameObject);
		}
		else if (col.gameObject.tag == "Food")
		{
			Destroy(col.gameObject);
		}
	}
}
