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

    internal int level = 0;
    public float coverageRequired = 20.0f;

    private float coverage = 0.0f;

    private Player player;

    public float CoverageProportion {
        get {
            return Mathf.Min(1.0f, coverage / coverageRequired);
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

        player = FindFirstObjectByType<Player>();

        state = GameState.Start;
        // TODO waiting for player to start 
        
        state = GameState.Playing;

        ResetLevel();
    }

    void ResetLevel() {
        Debug.Log("GameManager Start");

        level++;
        bubbles = initialBubbles;
        coverage = 0.0f;
        state = GameState.Playing;
        player.Respawn();

        for (var i = 0; i < enemyObjects.Count; i++) {
            try {
                Destroy(enemyObjects[i].gameObject);
            } catch (Exception e) {
                Debug.LogError("Error destroying enemy object: " + e.Message);
            }
        }
        enemyObjects.Clear();

        for (var i = 0; i < bubbleObjects.Count; i++) {
            try {
                Destroy(bubbleObjects[i].gameObject);
            } catch (Exception e) {
                Debug.LogError("Error destroying bubble object: " + e.Message);
            }
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
    }

    private void LevelComplete()
    {
        Debug.Log("GameManager LevelComplete");

        state = GameState.LevelComplete;

        StartCoroutine("ResetLevelDelayed");
    }

    private IEnumerator ResetLevelDelayed() {
        yield return new WaitForSeconds(2);

        ResetLevel();
    }

    private BubbleController activeBubble;

    // Set whilst the player is blowing up a bubble
    public void SetActiveBubble(BubbleController bubble)
    {
        activeBubble = bubble; 
    }

    public void BubbleCreated(BubbleController bubble)
    {
        bubbleObjects.Add(bubble);
        coverage += bubble.Area;
        bubbles--;
        if (CoverageProportion >= 1f) {
            state = GameState.LevelComplete;
            LevelComplete();
        } else if (bubbles <= 0) {
            GameOver();
        }
    }

    public void BubblePopped()
    {
        Debug.Log("GameManager BubblePopped");

        bubbles--;
        if (bubbles <= 0)
        {
            GameOver();
        }
        else
        {
            player.Hit();
        }
    }

    private void GameOver()
    {
        Debug.Log("GameManager GameOver");

        state = GameState.GameOver;
        player.Die();

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