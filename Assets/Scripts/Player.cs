using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement

    public Rigidbody2D body;

    public GameObject bubblePrefab;

    private BubbleController activeBubble;

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

        FlipForDirection(moveInput);

        BlowBubble();

        Debug.Log(body.linearVelocity);
    }

    private void FlipForDirection(float moveInput)
    {
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Blows a bubble.
    // If the player is not holding a bubble, creates a new one.
    // If the player is holding a bubble, inflates it.
    // If the player releases the bubble, the bubble will then collide with the player.
    private void BlowBubble()
    {
        if (activeBubble == null && Input.GetKeyDown(KeyCode.Space))
        {
            activeBubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity).GetComponent<BubbleController>();
            Physics2D.IgnoreCollision(activeBubble.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
        } else if (activeBubble != null) {
            if (Input.GetKey(KeyCode.Space)) {
                var scale = activeBubble.transform.localScale.x + 1f * Time.deltaTime;
                activeBubble.transform.localScale = new Vector3(scale, scale, scale);
                activeBubble.transform.position = transform.position;
            } else if (Input.GetKeyUp(KeyCode.Space)) {
                Physics2D.IgnoreCollision(activeBubble.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
                activeBubble = null;
            }
        }
    }
}
