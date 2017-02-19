using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour {

	private Text healthDisplay;
	private Player player;
	// Use this for initialization
	void Start () {
		healthDisplay = GetComponent<Text>();
		player = FindObjectOfType<Player>();
		UpdateDisplay();
	}
	
	public void UpdateDisplay()
	{
		healthDisplay.text = "Health: " + player.health;
	}
}
