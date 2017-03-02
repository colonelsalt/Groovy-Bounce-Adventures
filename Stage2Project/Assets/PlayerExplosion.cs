using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplosion : MonoBehaviour {

	private static PlayerExplosion instance = null;

	public AudioClip playerDeathSound;
	public float explosionTime, wiggleTime, sizeFactor;
	private Vector3 growthStep;
	private float timeBetweenGrowth, timePassed;
	private AudioSource audioSource;

	void Awake ()
	{
		if (instance) Destroy(gameObject);
		else instance = this;	

		audioSource = transform.parent.parent.gameObject.GetComponent<AudioSource>();
		transform.localScale = Vector3.one;
		growthStep = transform.parent.localScale * sizeFactor / 20f;
		timeBetweenGrowth = explosionTime / 20f;
		timePassed = 0;
		Invoke("InitParticles", explosionTime / 1.2f);
		transform.rotation = transform.parent.rotation;
	}

	void Update ()
	{
		if (timePassed >= timeBetweenGrowth)
		{
			transform.localScale = transform.localScale + growthStep;
			timePassed = 0;
		}
		timePassed += Time.deltaTime;
	}

	private void InitParticles()
	{
		Invoke("Hidesprite", explosionTime / 3f);
		GetComponent<ParticleSystem>().Play();
		audioSource.PlayOneShot(playerDeathSound);
	}

	private void Hidesprite()
	{
		GetComponent<Renderer>().enabled = false;
		instance = null;
		Destroy(gameObject, 2f);
	}
}
