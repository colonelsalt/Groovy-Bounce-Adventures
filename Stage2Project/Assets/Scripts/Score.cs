using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public static int score = 0;
	private Text scoreDisplay;

	void Start()
	{
		scoreDisplay = GetComponent<Text>();
		scoreDisplay.text = score.ToString();
	}
	
	public void IncrementScore(int amount)
	{
		score += amount;
		scoreDisplay.text = score.ToString();
	}
}
