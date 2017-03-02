using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum State { Paused, Playing }

    [SerializeField]
    private GameObject [] SpawnPrefabs;

    [SerializeField]
    private Player PlayerPrefab;

    [SerializeField]
    private Arena Arena;

    [SerializeField]
    private float TimeBetweenSpawns;

    public List<GameObject> mObjects;
    private Player mPlayer;
    private State mState;
    private float mNextSpawn;
    private bool gamePaused;

    private GameObject score, health, inventory, pauseScreen;

    void Awake()
    {
		mPlayer = Instantiate(PlayerPrefab);
        mPlayer.transform.parent = transform;

        score = GameObject.Find("Score");
        health = GameObject.Find("Health");
        inventory = GameObject.Find("Inventory");
        pauseScreen = GameObject.Find("PauseScreen");

        ScreenManager.OnNewGame += ScreenManager_OnNewGame;
        ScreenManager.OnExitGame += ScreenManager_OnExitGame;
    }

    void Start()
    {
        Arena.Calculate();
        SetUIVisibility(false);
        mState = State.Paused;
    }

    void Update()
    {
        if( mState == State.Playing)
        {
            mNextSpawn -= Time.deltaTime;
            if( mNextSpawn <= 0.0f )
            {
                if (mObjects == null)
                {
                    mObjects = new List<GameObject>();
                }

                int indexToSpawn = Random.Range(0, SpawnPrefabs.Length);
                GameObject spawnObject = SpawnPrefabs[indexToSpawn];
                GameObject spawnedInstance = Instantiate(spawnObject);
                spawnedInstance.transform.parent = transform;
                mObjects.Add(spawnedInstance);
                mNextSpawn = TimeBetweenSpawns;
            }
            if (Input.GetButton("Pause")) PauseScreen();
            if (gamePaused) PauseScreen();
        }
    }

    private void BeginNewGame()
    {
        if (mObjects != null)
        {
            for (int count = 0; count < mObjects.Count; ++count)
            {
                Destroy(mObjects[count]);
            }
            mObjects.Clear();
        }

        mPlayer.transform.position = new Vector3(4.5f, 3f, -55f);

        mNextSpawn = TimeBetweenSpawns;
        mPlayer.enabled = true;
        SetUIVisibility(true);
        health.GetComponent<HealthCounter>().ResetDisplay();
        score.GetComponent<Score>().ResetScore();
		mPlayer.Init();
        mState = State.Playing;
    }

    private void EndGame()
    {
        mPlayer.enabled = false;
        SetUIVisibility(false);
        mState = State.Paused;
    }

    private void SetUIVisibility(bool enabled)
    {
    	score.GetComponent<Text>().enabled = enabled;
    	inventory.GetComponent<RawImage>().enabled = enabled;
    	Transform healthTransform = health.GetComponent<Transform>();
    	foreach (Transform child in healthTransform)
    	{
    		child.gameObject.GetComponent<RawImage>().enabled = enabled;
    	}

		// inventory images should only be disabled (enabled on case-by-case basis in-game)
		if (!enabled)
    	{
    		Transform inventoryTransform = inventory.GetComponent<Transform>();
    		foreach (Transform child in inventoryTransform)
    		{
    			child.gameObject.GetComponent<RawImage>().enabled = false;
    		}
    	}
    }

    private void PauseScreen()
    {
    	gamePaused = true;
    	pauseScreen.GetComponent<Image>().enabled = true;
    	Time.timeScale = 0f;
		if (Input.GetButton("Submit"))
    	{
    		gamePaused = false;
			pauseScreen.GetComponent<Image>().enabled = false;
    		Time.timeScale = 1f;
    	}
    	else if (Input.GetButtonDown("Escape")) Application.Quit();
	}

    private void ScreenManager_OnNewGame()
    {
        BeginNewGame();
    }

    private void ScreenManager_OnExitGame()
    {
        EndGame();
    }
   
}
