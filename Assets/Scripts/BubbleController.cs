using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public bool IsBlowingUp = true;
    public float growSpeed = 0.7f;
    public float initialScale = 0.8f;

    private GameManager game;
    private Player player;
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
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBlowingUp) {
            game.SetActiveBubble(this);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // if another bubble and this bubble is blowing up, stop blowing up
        if (IsBlowingUp)
        {
            var otherBubble = collision.gameObject.GetComponent<BubbleController>();
            if (otherBubble != null)
            {
                player.StopBlowingBubble();
            }
        }
    }

    public void FinishBlowingUp()
    {
        IsBlowingUp = false;

        game.SetActiveBubble(null);
        game.BubbleCreated(this);

        AudioManager.PlayBubblePop();
        
        Debug.Log("Bubble created!");
    }

    public void Pop()
    {
        Debug.Log("Bubble destroyed!");

        game.BubblePopped();
        AudioManager.PlayBubbleDestroyed();
        
        Destroy(gameObject);
    }
}
