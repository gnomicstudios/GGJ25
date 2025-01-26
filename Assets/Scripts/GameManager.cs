using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int initialBubbles = 20;
    public int initialLives = 5;

    public FishController[] enemyPrefabs;

    internal GameState state = GameState.Start;
    private float timeAtStateChange = 0.0f;

    private List<FishController> enemyObjects = new List<FishController>();
    private List<BubbleController> bubbleObjects = new List<BubbleController>();
    
    internal int bubbles;

    internal int level = 0;
    public float coverageRequired = 20.0f;

    private float coverage = 0.0f;

    private Player player;

    private HudController hud;

    public float CoverageProportion {
        get {
            return Mathf.Min(1.0f, coverage / coverageRequired);
        }
    }

    void Awake()
    {
        Debug.Log("GameManager Awake");
        if (Instance != null) {
            Debug.LogError("GameManager already exists");
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameManager Start");

        player = FindFirstObjectByType<Player>();
        hud = FindFirstObjectByType<HudController>();
        SetState(GameState.Start);

        // Wait for player to start (in Update)
        hud.gameStartScreen.SlideIn();
    }

    private void SetState(GameState newState)
    {
        state = newState;
        timeAtStateChange = Time.time;
    }

    float TimeSinceStateChange
    {
        get { return Time.time - timeAtStateChange; }
    }

    void ResetLevel() {
        Debug.Log("GameManager Start");

        level++;
        bubbles = initialBubbles;
        coverage = 0.0f;
        SetState(GameState.Playing);
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

        switch (level) {
            case 1:
                LoadLevel1();
                break;
            case 2:
                LoadLevel2();
                break;
            case 3:
                LoadLevel3();
                break;
            case 4:
                LoadLevel4();
                break;
            case 5:
                LoadLevel5();
                break;
            case 6:
                LoadLevel6();
                break;
            case 7:
                LoadLevel7();
                break;
            case 8:
                LoadLevel8();
                break;
            default:
                LoadLevel8();
                break;
        }
    }

    void LoadLevel1() {
        var enemy = Instantiate(enemyPrefabs[0]);
        enemyObjects.Add(enemy);
    }

    void LoadLevel2() {
        var enemy = Instantiate(enemyPrefabs[7]);
        enemyObjects.Add(enemy);
    }

    void LoadLevel3() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
    }

    void LoadLevel4() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
    }

    void LoadLevel5() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
    }

    void LoadLevel6() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
    }

    void LoadLevel7() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
    }

    void LoadLevel8() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[7]));
    }

    IEnumerator ResetLevelDelayed() {
        yield return new WaitForSeconds(1f);
        ResetLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == GameState.Start) {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > 1f)
            {
                hud.gameStartScreen.SlideOut();
                StartCoroutine("ResetLevelDelayed");
            }
        }
        else if (state == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > 3f)
            {
                hud.gameOverScreen.SlideOut();
                StartCoroutine("ReloadGameDelayed");
            }
        }
    }

    private void LevelComplete()
    {
        Debug.Log("GameManager LevelComplete");

        SetState(GameState.LevelComplete);
        hud.levelCompleteScreen.SlideIn();
        StartCoroutine("NextLevelDelayed");
    }

    private IEnumerator NextLevelDelayed() {
        yield return new WaitForSeconds(3);
        hud.levelCompleteScreen.SlideOut();
        yield return new WaitForSeconds(1);
        ResetLevel();
    }

    public void BubbleCreated(BubbleController bubble)
    {
        bubbleObjects.Add(bubble);
        coverage += bubble.Area;
        bubbles--;
        if (CoverageProportion >= 1f) {
            SetState(GameState.LevelComplete);
            LevelComplete();
        } else if (bubbles <= 0) {
            SetState(GameState.GameOver);
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

        hud.gameOverScreen.SlideIn();
        SetState(GameState.GameOver);
        player.Die();
    }

    private IEnumerator ReloadGameDelayed() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainScene");
    }
}

public enum GameState {
    Start,
    Playing,
    GameOver,
    LevelComplete
}