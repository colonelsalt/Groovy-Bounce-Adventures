using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

	public enum Type {None, Star, Gun, Shield, Bomb, ExtraLife};
	public Type type;
	public float floatMagnitude, floatDistance;
	public AudioClip appearanceSound, collectSound;

	private AudioSource audioSource;
	private bool isColliding = false;
	private Rigidbody mBody;
	private float equilibirum;
	private Vector3 floatDirection;

	void Start()
	{
		audioSource = GetComponentInParent<AudioSource>();
		audioSource.PlayOneShot(appearanceSound);
		mBody = GetComponent<Rigidbody>();
		equilibirum = transform.position.z;
		floatDirection = Vector3.forward;
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag == "Player" && !isColliding)
		{
			audioSource.PlayOneShot(collectSound);
			isColliding = true;
			Player player = col.gameObject.GetComponent<Player>();
			player.AddPowerUp(type);
			Destroy(gameObject);
		}
	}

	void FixedUpdate()
	{
		mBody.AddForce(floatDirection * floatMagnitude);
		if (Mathf.Abs(transform.position.z - equilibirum) > floatDistance)
		{
			floatDirection = (transform.position.z <= equilibirum) ? Vector3.forward : Vector3.back;
		}
	}
				
}
