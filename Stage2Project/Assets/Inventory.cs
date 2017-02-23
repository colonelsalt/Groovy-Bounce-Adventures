using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	private Text text;
	private Player player;

	// Use this for initialization
	void Start ()
	{
		text = GetComponent<Text>();
		player = FindObjectOfType<Player>();
	}
	
	public void UpdateDisplay()
	{
		text.text = "( ";
		foreach (PowerUp.Type power in player.powerUps)
		{
			text.text += power.ToString() + " ";
		}
		text.text += ")\nActive power: " + player.currentPowerType;

	}
}
