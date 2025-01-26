using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public float growSpeed = 0.7f;
    public float initialScale = 0.8f;
    public AudioClip audioBubbleReleased;
    
    public GameObject bubbleExplosionPrefab;

    AudioSource audioSource;

    public bool IsBlowingUp
    {
        get { return player != null && player.GetActiveBubble() == this; }
    }

    private GameManager game;
    private Player player;
    private CircleCollider2D circleCollider;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    

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
        circleCollider = GetComponentInChildren<CircleCollider2D>();
        player = FindFirstObjectByType<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(bubbleBlueTime > 0f)
        {
            bubbleBlueTime += Time.deltaTime;
            if(bubbleBlueTime >= 0.4f)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    private float bubbleBlueTime = 0.0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if another bubble and this bubble is blowing up, stop blowing up
        if (IsBlowingUp)
        {
            var otherBubble = collision.gameObject.GetComponent<BubbleController>();
            if (otherBubble != null)
            {
                player.StopBlowingBubble();
                BubbleOnBubbleCollision();
                otherBubble.BubbleOnBubbleCollision();
            }
        }
    }

    public void BubbleOnBubbleCollision()
    {
        spriteRenderer.color = new Color(0f, 0.5f, 0.5f, 0.5f);
        bubbleBlueTime = 0.01f; 
    }

    public void FinishBlowingUp()
    {
        Debug.Log("Bubble created!");

        game.BubbleCreated(this);

        audioSource.PlayOneShot(audioBubbleReleased);
    }

    public void Pop()
    {
        Debug.Log("Bubble popped!");

        game.BubblePopped(this);

        circleCollider.enabled = false;

        var bubbleExplosion = Instantiate(bubbleExplosionPrefab, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }

}
