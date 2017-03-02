using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public int Damage;
	public int ScoreValue;
	public AudioClip damageSound;

	private Score score;
	private Player player;
	private AudioSource audioSource;

	void Start()
	{
		score = FindObjectOfType<Score>();
		player = FindObjectOfType<Player>();
		audioSource = GetComponentInParent<AudioSource>();
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (player.currentPowerType == PowerUp.Type.Star && Input.GetButton("Fire1")) Die();
			else player.TakeDamage();
		}
		else if (col.gameObject.tag == "Projectile") Die();
	}

	void OnParticleCollision(GameObject particle)
	{
		if (particle.tag == "Explosion") Die();
	}

	private void Die()
	{
		audioSource.PlayOneShot(damageSound);
		score.IncrementScore(ScoreValue);
		Destroy(gameObject);
	}

}
