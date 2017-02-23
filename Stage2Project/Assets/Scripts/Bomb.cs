using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

	public float speed;
	public float slowMotionTime;
	public GameObject explosionPrefab;

	private Player player;
	private PowerUpTimer timerDisplay;
	private float timer;

	void Awake()
	{
		player = FindObjectOfType<Player>();
		timerDisplay = FindObjectOfType<PowerUpTimer>();
	}

	void Start ()
	{
		SlowDownTime();
		player.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButton("Horizontal"))
		{
			transform.position += Input.GetAxisRaw("Horizontal") * Vector3.right * speed * Time.deltaTime;
		}
		if (Input.GetButton("Vertical"))
		{
			transform.position += Input.GetAxisRaw("Vertical") * Vector3.forward * speed * Time.deltaTime;
		}
		if (Input.GetButtonDown("Fire1"))
		{
			timer = slowMotionTime;
			timerDisplay.ResetDisplay();
			Instantiate(explosionPrefab, transform.position, Quaternion.identity);
			ReturnTimeToNormal();
		}
		timer += Time.deltaTime;
		timerDisplay.UpdateDisplay(slowMotionTime - timer);
		if (timer > slowMotionTime)
		{
			ReturnTimeToNormal();
		}
	}

	private void SlowDownTime()
	{
		// TODO: add motion blur + black and white camera effect here
		Time.timeScale = 0.04f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		timer = 0f;
	}

	private void ReturnTimeToNormal()
	{
		Time.timeScale = 1f;
		Time.fixedDeltaTime = 0.02f * Time.timeScale;
		player.enabled = true;
		player.ResumeMovement();
		Destroy(gameObject);
	}
}
