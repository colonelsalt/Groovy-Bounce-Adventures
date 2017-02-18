using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalWall : MonoBehaviour {

	public static float Width;
	public static float Height;

	// Use this for initialization
	void Start () {
		Width = GetComponent<Collider>().bounds.size.x;
		Height = GetComponent<Collider>().bounds.size.z;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
