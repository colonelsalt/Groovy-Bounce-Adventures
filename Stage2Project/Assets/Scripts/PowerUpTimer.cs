using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimer : MonoBehaviour {

	Text text;

	void Start ()
	{
		text = GetComponent<Text>();
		UpdateDisplay(0f);
	}
	
	public void UpdateDisplay(float remainingTime)
	{
		if (remainingTime > 0) text.text = "Powerup time: \n" + remainingTime.ToString();
		else text.text = "";
	}

	public void ResetDisplay()
	{
		text.text = "";
	}
}
