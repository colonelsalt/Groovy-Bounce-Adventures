using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

	public int score = 0;
	private Text scoreDisplay;
	private Animator animator;

	void Start()
	{
		scoreDisplay = GetComponent<Text>();
		animator = GetComponent<Animator>();
		scoreDisplay.text = score.ToString();
	}
	
	public void IncrementScore(int amount)
	{
		score += amount;
		scoreDisplay.text = score.ToString();
		animator.SetTrigger("RegularFlash");
	}

	public void ResetScore()
	{
		score = 0;
	}
}
