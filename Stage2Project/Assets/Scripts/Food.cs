using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

	public int ScoreValue;
	private Score score;

	void Awake()
	{
		score = FindObjectOfType<Score>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			score.IncrementScore(ScoreValue);
			Destroy(gameObject);
		}
	}
}
