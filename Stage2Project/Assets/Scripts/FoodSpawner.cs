using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

	public GameObject[] foodPrefabs;

	void Awake ()
	{
		GameObject food = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
		food.transform.parent = transform.parent;
		Destroy(gameObject);
	}
}
