using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour {

	public float destructionDelay;

	void Start ()
	{
		Destroy(gameObject, GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + destructionDelay);
	}
}
