using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalWall : MonoBehaviour {

	public static float Width;
	public static float Height;
	public static float Depth;

	// Use this for initialization
	void Start () {
		Width = GetComponent<Collider>().bounds.size.x;
		Height = GetComponent<Collider>().bounds.size.z;
		Depth = GetComponent<Collider>().bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
