using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour {

	private Quaternion startRotation;

	void Awake ()
	{
		startRotation = transform.rotation;		
	}

	void Update()
	{
		transform.position = new Vector3(transform.parent.position.x, transform.parent.position.y +
									(transform.parent.localScale.y / 2f), transform.parent.position.z);
	}

	void LateUpdate ()
	{
		transform.rotation = startRotation;
	}
}
