using System.Drawing;
using UnityEngine;

public class FishController : MonoBehaviour
{
    public Rect tankBounds; // The boundaries of the tank (x1, y2, x2, y2 dimensions)
    public float swimSpeed = 2f; // Speed at which the fish swims
    public float directionChangeInterval = 3f; // Time interval for changing direction

    private Vector2 targetDirection; // The direction the fish is currently swimming
    private float directionChangeTimer; // Timer for changing direction

    void Start()
    {
        // Set an initial random direction
        targetDirection = GetRandomDirection();
        directionChangeTimer = directionChangeInterval;
    }

    void Update()
    {
        // Move the fish in the current direction
        transform.Translate(targetDirection * swimSpeed * Time.deltaTime, Space.World);

        // Check if the fish is close to the tank bounds and adjust direction
        StayWithinBounds();

        // Update the timer and change direction if needed
        directionChangeTimer -= Time.deltaTime;
        if (directionChangeTimer <= 0f)
        {
            targetDirection = GetRandomDirection();
            directionChangeTimer = directionChangeInterval;
        }
    }

    // Ensure the fish stays within the tank bounds
    private void StayWithinBounds()
    {
        Vector2 position = transform.position;

        if (tankBounds.Contains(position) == false)
        {
            // Reverse direction when hitting the bounds
            targetDirection = -targetDirection;

            // Clamp the position to ensure it stays within bounds
            position.x = Mathf.Clamp(position.x, tankBounds.x, tankBounds.x + tankBounds.width);
            position.y = Mathf.Clamp(position.y, tankBounds.y - tankBounds.height, tankBounds.y);
            transform.position = position;
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
}
