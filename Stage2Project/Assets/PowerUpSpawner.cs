using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

	public GameObject[] powerUpPrefabs;

	void Start()
	{
		GameObject powerUp = Instantiate(powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)]);
		powerUp.GetComponent<Transform>().parent = transform.parent;
		FindObjectOfType<GameManager>().mObjects.Add(powerUp);
		Destroy(gameObject);
	}
}
