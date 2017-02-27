using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum Type {None, Star, Gun, Shield, Bomb, ExtraLife};
	public Type type;
	private bool isColliding = false;
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && !isColliding)
		{
			isColliding = true;
			Player player = col.gameObject.GetComponent<Player>();
			player.AddPowerUp(type);
			Destroy(gameObject);
		}
	}
		
}
