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
			Player player = col.gameObject.GetComponent<Player>();
			//if (player.currentPowerType == PowerUp.Type.Hammer && )
			score.IncrementScore(ScoreValue);
			Destroy(gameObject);
		}
	}
}
