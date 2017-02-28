using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileChange : MonoBehaviour {

	public Material[] tilesets;
	public float switchTime;
	private Renderer renderer;
	private float timeSinceSwitch;
	private int tileIndex = 1;

	void Awake()
	{
		renderer = GetComponent<Renderer>();
		timeSinceSwitch = 0f;
	}

	void Update()
	{
		timeSinceSwitch += Time.deltaTime;
		if (timeSinceSwitch >= switchTime)
		{
			timeSinceSwitch = 0f;
			renderer.material = tilesets[tileIndex];
			tileIndex = (tileIndex + 1) % tilesets.Length;
		}	
	}
}
