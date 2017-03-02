using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpPrefabs;

	void Awake ()
	{ // TODO: OBVS
		//GameObject powerUp = Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)]);
		GameObject powerUp = Instantiate(powerUpPrefabs[0]);
		powerUp.transform.parent = transform.parent;
		Destroy(gameObject);
	}
}
