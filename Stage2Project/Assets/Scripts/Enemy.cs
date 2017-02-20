using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int Damage;
	public int ScoreValue;

	// Use this for initialization
	void Start () {
		
	}
	
	void OnParticleTrigger()
	{
		Destroy(gameObject);
	}
}
