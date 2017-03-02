using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour {

	public GameObject[] foodPrefabs;

	void Start()
	{
		GameObject food = Instantiate(foodPrefabs[Random.Range(0, foodPrefabs.Length)]);
		food.GetComponent<Transform>().parent = transform.parent;
		FindObjectOfType<GameManager>().mObjects.Add(food);
		Destroy(gameObject);
	}
}
