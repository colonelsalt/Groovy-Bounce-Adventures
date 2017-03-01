using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimer : MonoBehaviour {

	public float TIME_FACTOR;

	private GameObject barHolder;
	private Scrollbar timeBar;
	private RawImage activePowerImage;
	private InventoryDisplay inventory;
	private Player player;
	private bool timerActive = false;

	void Start ()
	{
		barHolder = transform.GetChild(0).gameObject;
		timeBar = barHolder.GetComponent<Scrollbar>();
		activePowerImage = transform.GetChild(1).gameObject.GetComponent<RawImage>();
		EnableDisplay(false);
		inventory = FindObjectOfType<InventoryDisplay>();
		player = FindObjectOfType<Player>();
	}

	public void StartTimer()
	{
		timerActive = true;
		timeBar.size = 1;
		switch (player.currentPowerType)
		{
			case PowerUp.Type.Bomb:
				activePowerImage.texture = inventory.bombSprite[0];
				break;
			case PowerUp.Type.Gun:
				activePowerImage.texture = inventory.gunSprite[0];
				break;
			case PowerUp.Type.Shield:
				activePowerImage.texture = inventory.shieldSprite[0];
				break;
			case PowerUp.Type.Star:
				activePowerImage.texture = inventory.starSprite[0];
				break;
		}
		EnableDisplay(true);
	}

	public void CancelTimer()
	{
		timerActive = false;
		EnableDisplay(false);
		player.CancelPowerUp();
	}

	private void EnableDisplay(bool enabled)
	{
		activePowerImage.enabled = enabled;
		barHolder.GetComponent<Image>().enabled = enabled;
		barHolder.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().enabled = enabled;
		timeBar.enabled = enabled;
	}

	void Update()
	{
		if (timeBar.size > 0 && timerActive)
		{
			timeBar.size -= TIME_FACTOR * Time.deltaTime;
		}
		else if (timerActive)
		{
			CancelTimer();
		}
	}
}
