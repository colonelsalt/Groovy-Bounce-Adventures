using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpPrefabs;

	void Awake ()
	{
		GameObject powerUp = Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)]);
		powerUp.transform.parent = transform.parent;
		Destroy(gameObject);
	}
}
