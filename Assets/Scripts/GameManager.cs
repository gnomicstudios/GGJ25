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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
