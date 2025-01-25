using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int initialBubbles = 20;
    public int initialLives = 5;

    public FishController[] enemyPrefabs;

    internal GameState state = GameState.Start;

    private List<FishController> enemyObjects = new List<FishController>();
    private List<BubbleController> bubbleObjects = new List<BubbleController>();
    

    internal int bubbles;
    internal int lives;


    internal int level = 1;
    public float coverageRequired = 20.0f;



    private float coverage = 0.0f;

    internal float coverageExtra = 0.0f;

    public float CoverageProportion {
        get {
            return Mathf.Min(1.0f, (coverage + coverageExtra) / coverageRequired);
        }
    }

    private float CoveragePropotionLockedIn {
        get {
            return Mathf.Min(1.0f, coverage / coverageRequired);
        }
    }

    void Awake()
    {
        Debug.Log("GameManager Awake");
        //DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameManager Start");

        state = GameState.Start;
        // TODO waiting for player to start 
        
        state = GameState.Playing;

        ResetLevel();
    }

    void ResetLevel() {
        Debug.Log("GameManager Start");

        bubbles = initialBubbles;
        lives = initialLives;
        coverage = 0.0f;
        coverageExtra = 0.0f;
        state = GameState.Playing;

        for (var i = 0; i < enemyObjects.Count; i++) {
            Destroy(enemyObjects[i].gameObject);
        }
        enemyObjects.Clear();

        for (var i = 0; i < bubbleObjects.Count; i++) {
            Destroy(bubbleObjects[i].gameObject);
        }
        bubbleObjects.Clear();

        // One enemy extra per level
        for (var i = 0; i < level; i++) {
            var enemy = Instantiate(enemyPrefabs[i % enemyPrefabs.Length]);
            enemyObjects.Add(enemy);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Playing) {
            if (CoveragePropotionLockedIn >= 1f) {
                state = GameState.LevelComplete;
                LevelComplete();
            }
        }
    }

    private void LevelComplete()
    {
        Debug.Log("GameManager LevelComplete");

        state = GameState.LevelComplete;

        StartCoroutine("ResetLevelDelayed");
    }

    private IEnumerator ResetLevelDelayed() {
        yield return new WaitForSeconds(2);
        level++;
        ResetLevel();
    }

    // Set whilst the player is blowing up a bubble
    public void SetBubbleBlowing(float area)
    {
        coverageExtra = area; 
    }

    public void BubbleCreated(BubbleController bubble)
    {
        bubbleObjects.Add(bubble);
        coverage += bubble.Area;
        bubbles--;  
    }

    public void BubblePopped()
    {
        Debug.Log("GameManager BubblePopped");

        coverageExtra = 0f;
        lives--;
        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GameManager GameOver");

        state = GameState.GameOver;

        StartCoroutine("ReloadGameDelayed");
    }

    private IEnumerator ReloadGameDelayed() {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("MainScene");
    }
}

public enum GameState {
    Start,
    Playing,
    GameOver,
    LevelComplete
}