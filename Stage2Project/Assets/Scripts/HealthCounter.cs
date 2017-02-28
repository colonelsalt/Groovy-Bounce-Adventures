using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCounter : MonoBehaviour {
	private int heartIndex = 0;

	public void DecrementDisplay()
	{
		Transform heart = transform.GetChild(heartIndex++);
		heart.gameObject.GetComponent<RawImage>().enabled = false;
	}
	public void IncrementDisplay()
	{
		Transform heart = transform.GetChild(--heartIndex);
		heart.gameObject.GetComponent<RawImage>().enabled = true;
	}
}
