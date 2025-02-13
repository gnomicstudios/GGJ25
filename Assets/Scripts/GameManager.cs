using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int initialBubbles = 20;

    public FishController[] enemyPrefabs;
    public GameObject winPrefab;
    public AudioSource music;



    internal GameState state = GameState.Start;
    private float timeAtStateChange = 0.0f;

    private List<FishController> enemyObjects = new List<FishController>();
    private List<BubbleController> bubbleObjects = new List<BubbleController>();
    private Transform winScreen;
    
    // Current number of bubbles
    internal int bubbles;

    // The current level
    public int level = 0;

    public float coverageRequired = 20.0f;

    // Total coverage of bubbles in area
    private float coverage = 0.0f;

    // Time the UI screens take to slide in and out
    private float slideTime = 0.8f;

    private float timeAtLevelStart = 0.0f;
    internal float timeTotalAllLevels = 0.0f;
    public float TimeTotal {
        get {
            if (state == GameState.Playing) {
                return timeTotalAllLevels + Time.time - timeAtLevelStart;
            } else {
                return timeTotalAllLevels;
            }
        }
    }
    internal int bubblesTotalUsed = 0;

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
        hud.gameStartScreen.SlideIn(slideTime);
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

    void ClearLevel() {
        Debug.Log("GameManager ClearLevel");

        for (var i = 0; i < enemyObjects.Count; i++) {
            try {
                Destroy(enemyObjects[i].gameObject);
            } catch (Exception e) {
                Debug.LogWarning("Error destroying enemy object: " + e.Message);
            }
        }
        enemyObjects.Clear();

        for (var i = 0; i < bubbleObjects.Count; i++) {
            try {
                Destroy(bubbleObjects[i].gameObject);
            } catch (Exception e) {
                Debug.LogWarning("Error destroying bubble object: " + e.Message);
            }
        }
        bubbleObjects.Clear();
    }

    void ResetLevel() {
        Debug.Log("GameManager ResetLevel");

        level++;
        bubbles = initialBubbles;
        coverage = 0.0f;
        timeAtLevelStart = Time.time;

        // Win level if we've reached the end
        if (level > 9) {
            LoadWinLevel();
            SetState(GameState.GameComplete);
            music.volume = 0.9f;
            player.gameObject.SetActive(false);
            LeanTween.scale(hud.endingUI, Vector3.one * 2f, 1f).setEaseOutBounce();
            return;
        }


        SetState(GameState.Playing);
        player.Respawn();

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
            case 9:
                LoadLevel9();
                break;
            default:
                LoadWinLevel();
                break;
        }
    }

    void LoadLevel1() {
        var enemy = Instantiate(enemyPrefabs[0]);
        enemyObjects.Add(enemy);
    }

    void LoadLevel2() {
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
    }

    void LoadLevel3() {
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
    }

    void LoadLevel4() {
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
    }

    void LoadLevel5() {
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
    }

    void LoadLevel6() {
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
    }

    void LoadLevel7() {
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
    }

    void LoadLevel8() {
        enemyObjects.Add(Instantiate(enemyPrefabs[0]));
        enemyObjects.Add(Instantiate(enemyPrefabs[1]));
        enemyObjects.Add(Instantiate(enemyPrefabs[2]));
        enemyObjects.Add(Instantiate(enemyPrefabs[3]));
        enemyObjects.Add(Instantiate(enemyPrefabs[4]));
        enemyObjects.Add(Instantiate(enemyPrefabs[5]));
        enemyObjects.Add(Instantiate(enemyPrefabs[6]));
    }

    void LoadLevel9() {
        var superStar = Instantiate(enemyPrefabs[7]);
        superStar.MaxSpeed = 6.0f;
        superStar.Acceleration = 8f;
        superStar.targettingSpeedModifer = 1.3f;
        superStar.directionChangeInterval = 0.3f;
        superStar.targetOnDirectionChangeChance = 1.0f;
        enemyObjects.Add(superStar);
    }

    void LoadWinLevel() {
        winScreen = Instantiate(winPrefab).transform;
    }

    IEnumerator ResetLevelDelayed() {
        yield return new WaitForSeconds(slideTime);
        ResetLevel();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: show menu once we have a menu, for now just exit fullscreen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Screen.fullScreen = false; 
        }

        if (state == GameState.Transitioning) {
            return;
        }

        if (state == GameState.Start)
        {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > slideTime)
            {
                hud.gameStartScreen.SlideOut(slideTime);
                SetState(GameState.Transitioning);
                ClearLevel();
                StartCoroutine("ResetLevelDelayed");
            }
        }
        else if (state == GameState.LevelComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > slideTime)
            {
                hud.levelCompleteScreen.SlideOut(slideTime);
                SetState(GameState.Transitioning);
                ClearLevel();
                StartCoroutine("ResetLevelDelayed");
            }
        }
        else if (state == GameState.GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > 3f)
            {
                hud.gameOverScreen.SlideOut(slideTime);
                SetState(GameState.Transitioning);
                ClearLevel();
                StartCoroutine("ReloadGameDelayed");
            }
        }
        else if (state == GameState.GameComplete)
        {
            if (Input.GetKeyDown(KeyCode.Space) && TimeSinceStateChange > 3f)
            {
                LeanTween.scale(winScreen.gameObject, Vector3.one * 5f, slideTime * 1.5f).setEaseInOutCubic();
                SetState(GameState.Transitioning);
                ClearLevel();
                StartCoroutine("ReloadGameDelayed");
            }
        }
    }

    private void LevelComplete()
    {
        Debug.Log("GameManager LevelComplete");

        timeTotalAllLevels += Time.time - timeAtLevelStart;
        SetState(GameState.LevelComplete);
        hud.levelCompleteScreen.SlideIn(slideTime);
    }

    public void BubbleCreated(BubbleController bubble)
    {
        bubbleObjects.Add(bubble);
        coverage += bubble.Area;
        bubbles--;
        bubblesTotalUsed++;
        if (CoverageProportion >= 1f) {
            SetState(GameState.LevelComplete);
            LevelComplete();
        } else if (bubbles <= 0) {
            SetState(GameState.GameOver);
            GameOver();
        }
    }

    public void BubblePopped(BubbleController bubble)
    {
        Debug.Log("GameManager BubblePopped");

        player.Hit();
        bubbles--;
        bubblesTotalUsed++;
        if (bubbles <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GameManager GameOver");

        hud.gameOverScreen.SlideIn(slideTime);
        SetState(GameState.GameOver);
        player.Die();
    }

    private IEnumerator ReloadGameDelayed() {
        yield return new WaitForSeconds(slideTime);
        SceneManager.LoadScene("MainScene");
    }
}

public enum GameState {
    Start,
    Playing,
    LevelComplete,
    GameOver,
    GameComplete,
    Transitioning, // Used to prevent input during transition
}