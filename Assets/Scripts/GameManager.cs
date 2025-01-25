using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int bubbles = 20;
    public int lives = 5;
    public float coverage = 0.0f;

    private float totalArea = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BubbleCreated(float area)
    {
        coverage += area;
        bubbles--;  
    }

    public void BubblePopped()
    {
        lives--;
        if (lives <= 0)
        {
            //GameOver();
        }
    }   
}
