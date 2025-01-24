using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement

    public Rigidbody2D body;

    private void Start()
    {
        // Get the Rigidbody2D component attached to the player
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Get horizontal input (A/D keys or Left/Right arrow keys)
        float moveInput = Input.GetAxis("Horizontal");

        // Calculate the new velocity
        Vector2 velocity = new Vector2(moveInput * speed, body.linearVelocity.y);

        // Apply the velocity to the Rigidbody2D
        body.linearVelocity = velocity;

        Debug.Log(body.linearVelocity);
    }
}
