using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum Type {None, Hammer, Gun, Shield, Bomb};
	/*
		-Shield is one-time activated, and simply prevents you from taking damage.
		-Hammer is activated on per-press basis (but timer starts running from first press); it prevents damages
		 and kills enemies in your path, but makes you slower.
		-Gun lets you fire projectiles (in direction indicated by arrow keys) that kills enemies in path.
	*/
	private Type type;

	// Use this for initialization
	void Start () {
		type = Type.Hammer;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			Player player = col.gameObject.GetComponent<Player>();
			player.SetPowerUp(type);
			Destroy(gameObject);
		}
	}
		
}
