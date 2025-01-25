using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public bool IsBlowingUp = true; 

    private GameManager game;
    private CircleCollider2D circleCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = FindFirstObjectByType<GameManager>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishBlowingUp()
    {
        IsBlowingUp = false;

        // Calculate the area using the radius
        float radius = circleCollider.radius;
        float area = Mathf.PI * Mathf.Pow(radius, 2);

        game.BubbleCreated(area);

        AudioManager.PlayBubblePop();
        
        Debug.Log("Bubble destroyed!");
    }

    public void Pop()
    {
        Debug.Log("Bubble destroyed!");

        game.BubblePopped();
        AudioManager.PlayBubbleDestroyed();
        
        Destroy(gameObject);
    }
}
