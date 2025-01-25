using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int bubbles = 20;
    public int lives = 5;
    public int level = 1;
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
        DontDestroyOnLoad(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameManager Start");
    }

    // Update is called once per frame
    void Update()
    {
        if (CoveragePropotionLockedIn > 1f) {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        Debug.Log("GameManager LevelComplete");
    }

    // Set whilst the player is blowing up a bubble
    public void SetBubbleBlowing(float area)
    {
        coverageExtra = area; 
    }

    public void BubbleCreated(float area)
    {
        coverage += area;
        bubbles--;  
    }

    public void BubblePopped()
    {
        coverageExtra = 0f;
        lives--;
        if (lives <= 0)
        {
            //GameOver();
        }
    }   
}
