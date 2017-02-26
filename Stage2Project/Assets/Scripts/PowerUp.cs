using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum Type {None, Hammer, Gun, Shield, Bomb, ExtraLife};
	/*
		-Shield is one-time activated, and simply prevents you from taking damage.
		-Hammer is activated on per-press basis (but timer starts running from first press); it prevents damages
		 and kills enemies in your path, but makes you slower.
		-Gun lets you fire projectiles (in direction indicated by arrow keys) that kills enemies in path.
	*/
	private Type type;
	private bool isColliding = false;

	// Use this for initialization
	void Start ()
	{
		type = (Type) Random.Range((int)Type.Hammer, (int)Type.ExtraLife + 1);
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && !isColliding)
		{
			isColliding = true;
			Debug.Log("PowerUp's OnTriggerEnter called!");
			Player player = col.gameObject.GetComponent<Player>();
			player.AddPowerUp(type);
			Destroy(gameObject);
		}
	}
		
}
