using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public bool IsBlowingUp = true;
    public float growSpeed = 0.7f;
    public float initialScale = 0.8f;

    private GameManager game;
    private CircleCollider2D circleCollider;

    // Calculate the area using the radius
    public float Area {
        get {
            var scale = transform.localScale.magnitude;
            return Mathf.PI * Mathf.Pow(circleCollider.radius * scale, 2);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        game = FindFirstObjectByType<GameManager>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBlowingUp) {
            game.SetBubbleBlowing(Area);
        }
    }

    public void FinishBlowingUp()
    {
        IsBlowingUp = false;

        game.SetBubbleBlowing(0.0f);
        game.BubbleCreated(Area);

    }

    public void Pop()
    {
        // Play a popping animation
        // Destroy the bubble object
        Destroy(gameObject);
        game.BubblePopped();

    }
}
