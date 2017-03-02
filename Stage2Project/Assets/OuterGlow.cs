using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OuterGlow : MonoBehaviour {

	public float destructionTime;
	public float glowSizeFactor;
	private Vector3 growthStep;
	private float timeBetweenGrowth, timePassed;

	// Use this for initialization
	void Awake ()
	{
		transform.localScale = Vector3.one;
		growthStep = transform.parent.localScale * glowSizeFactor / 10f;
		timeBetweenGrowth = destructionTime / 10f;
		timePassed = 0;
		Destroy(gameObject, destructionTime);
		transform.rotation = transform.parent.rotation;
	}

	void Update ()
	{
		if (timePassed >= timeBetweenGrowth)
		{
			transform.localScale = transform.localScale + growthStep;
			timePassed = 0;
		}
		transform.position = transform.parent.position;
		transform.rotation = transform.parent.rotation;
		timePassed += Time.deltaTime;
	}
}
