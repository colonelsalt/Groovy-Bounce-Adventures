using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum Type {None, Hammer};
	private Type type;

	// Use this for initialization
	void Start () {
		type = Type.Hammer;
	}
	
	void OnTriggerEnter(Collider col)
	{
		Debug.Log("PowerUp trigger with " + col.gameObject.tag);
		if (col.gameObject.tag == "Player")
		{
			Player player = col.gameObject.GetComponent<Player>();
			player.SetPowerUp(type);
			Destroy(gameObject);
		}
	}
		
}
