using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
	public Texture[] bombSprite, gunSprite, shieldSprite, starSprite;
	private GameObject[] slots;
	private Player player;

	// Use this for initialization
	void Start ()
	{
		player = FindObjectOfType<Player>();
		slots = new GameObject[3];
		int i = 0;
		foreach (Transform child in transform) slots[i++] = child.gameObject;
	}
	
	public void UpdateDisplay()
	{
		for (int i = 0; i < slots.Length; i++)
		{
			RawImage currentImage = slots[i].GetComponent<RawImage>();
			switch (player.powerUps[i])
			{
				case PowerUp.Type.None:
					currentImage.enabled = false;
					break;
				case PowerUp.Type.Bomb:
					currentImage.enabled = true;
					currentImage.texture = (player.powerIndex == i) ? bombSprite[1] : bombSprite[0];
					break;
				case PowerUp.Type.Gun:
					currentImage.enabled = true;
					currentImage.texture = (player.powerIndex == i) ? gunSprite[1] : gunSprite[0];
					break;
				case PowerUp.Type.Shield:
					currentImage.enabled = true;
					currentImage.texture = (player.powerIndex == i) ? shieldSprite[1] : shieldSprite[0];
					break;
				case PowerUp.Type.Star:
					currentImage.enabled = true;
					currentImage.texture = (player.powerIndex == i) ? starSprite[1] : starSprite[0];
					break;
			}
		}
	}
}
