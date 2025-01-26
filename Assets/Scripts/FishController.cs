using UnityEngine;

public class FishController : MonoBehaviour
{
    public float MaxSpeed = 3f; // Speed at which the fish swims
    public float Acceleration = 0.1f; // Acceleration of the fish 
    public float directionChangeInterval = 3f; // Time interval for changing direction
    public float targetOnDirectionChangeChance = 0.5f; // Chance of targeting the active bubble when changing direction
    public float targettingSpeedModifer = 1.0f; // Speed modifier when targetting the active bubble

    private Rigidbody2D rb; // Reference to the fish's rigidbody
    private Vector2 targetDirection; // The direction the fish is currently swimming
    private float directionChangeTimer; // Timer for changing direction
    private Player player;

    void Start()
    {
        player = FindFirstObjectByType<Player>();

        // Set spawn pos
        transform.position = GetRandomSpawn();

        // Set an initial random direction
        rb = GetComponent<Rigidbody2D>();
        targetDirection = GetRandomDirection();
        directionChangeTimer = directionChangeInterval * 0.2f;
    }

    bool isTargettingBubble = false;
    void Update()
    {
        // Update the timer and change direction if needed
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            Vector2 bubbleDirection;
            bool gotBubbleDirection = TryGetBubbleDirection(out bubbleDirection);

            if (gotBubbleDirection && (isTargettingBubble || Random.value < targetOnDirectionChangeChance))
            {
                targetDirection = bubbleDirection;
                isTargettingBubble = true;
            }
            else
            {
                targetDirection = GetRandomDirection();
                isTargettingBubble = false;
            }
            directionChangeTimer = directionChangeInterval;
        }

        // rb.AddForce(targetDirection * Acceleration);
        // rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity, MaxSpeed);
        rb.linearVelocity = targetDirection * MaxSpeed;
        if (isTargettingBubble)
        {
            rb.linearVelocity *= targettingSpeedModifer;
        }

        // Flip the fish sprite based on the direction it is swimming
        var scale = transform.localScale;
        if (targetDirection.x > 0)
        {
            // Facing RIGHT
            if (scale.x < 0f) {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }
        else if (targetDirection.x < 0)
        {
            // Facing LEFT
            if (scale.x > 0f) {
                scale.x *= -1f;
                transform.localScale = scale;
            }
        }

        // // Rotate towards the target direction
        // float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has a specific component (e.g., ObstacleController)
        var bubble = collision.gameObject.GetComponent<BubbleController>();
        if (bubble != null && bubble.IsBlowingUp)
        {
            bubble.Pop();
        }
        else    
        {
            // Reverse direction upon collision and take off some speed
            //rb.linearVelocity = -(rb.linearVelocity * 0.75f);
            targetDirection = -targetDirection;
        }
    }


    // Generate a random direction for the fish to swim
    private Vector2 GetRandomDirection()
    {
        return new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    private bool TryGetBubbleDirection(out Vector2 direction)
    {
        var activeBubble = player.GetActiveBubble();
        if (activeBubble == null)
        {
            direction = Vector2.zero;
            return false;
        }

        // // check not too far, i.e. fish sees the bubble
        // if (Vector2.Distance(activeBubble.transform.position, transform.position) > 7f)
        // {
        //     direction = Vector2.zero;
        //     return false;
        // }

        direction = (activeBubble.transform.position - transform.position).normalized;
        return true;
    }

    private Vector2 GetRandomSpawn()
    {
        return new Vector2(
            Random.Range(-4f, 4f),
            Random.Range(1f, 4f)
        );
    }
}
