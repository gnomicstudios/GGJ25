using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement

    public GameObject bubblePrefab;
    public AudioClip audioBubbleStrech;

    private BubbleController activeBubble;
    private SpringJoint2D activeBubbleSpring;
    private AudioSource audioSource;


    internal Rigidbody2D body;
    internal Animator animator;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        activeBubbleSpring = GetComponent<SpringJoint2D>();
        activeBubbleSpring.enabled = false;
        body.gravityScale = 0.2f;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (GameManager.Instance.state == GameState.Playing )
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveY = Input.GetAxis("Vertical");

            MovePlayer(moveX, moveY);
            FlipForDirection(moveX);

            UpdateActiveBubble();
        }

        animator.SetBool("isBlowing", activeBubble != null);
    }

    private void MovePlayer(float moveX, float moveY)
    {
        float velocityX = moveX * speed;
        float velocityY = moveY * speed;

        // Apply the velocity to the Rigidbody2D, Y value is only applied if it's non-zero
        body.linearVelocity = new Vector2(
            velocityX,
            Math.Abs(velocityY) > Mathf.Epsilon ? velocityY : body.linearVelocity.y
        );

        // Apply the velocity to the bubble
        if (activeBubble != null) {
            var bubbleBody = activeBubble.GetComponent<Rigidbody2D>();
            bubbleBody.linearVelocity = new Vector2(velocityX, velocityY) * 1.2f;
        }
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
    private void UpdateActiveBubble()
    {
        if (activeBubble == null && Input.GetKeyDown(KeyCode.Space))
        {
            activeBubble = Instantiate(bubblePrefab, transform.position, Quaternion.identity).GetComponent<BubbleController>();
            activeBubble.transform.localScale = new Vector3(activeBubble.initialScale, activeBubble.initialScale, activeBubble.initialScale);
            activeBubbleSpring.connectedBody = activeBubble.GetComponent<Rigidbody2D>();
            activeBubbleSpring.enabled = true;
            audioSource.PlayOneShot(audioBubbleStrech);
            Physics2D.IgnoreCollision(activeBubble.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), true);
        }
        else if (activeBubble != null)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                var scale = activeBubble.transform.localScale.x + activeBubble.growSpeed * Time.deltaTime;
                activeBubble.transform.localScale = new Vector3(scale, scale, scale);
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                StopBlowingBubble();
            }
        }
    }

    public void StopBlowingBubble()
    {
        if(activeBubble == null) return;

        Physics2D.IgnoreCollision(activeBubble.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(), false);
        activeBubble.FinishBlowingUp();
        CancelBubble();
        audioSource.Stop();
    }


    private void CancelBubble()
    {
        activeBubble = null;
        activeBubbleSpring.enabled = false;
        activeBubbleSpring.connectedBody = null;
    }

    public BubbleController GetActiveBubble()
    {
        return activeBubble;
    }

    public void BubblePopped()
    {
        audioSource.Stop();
    }

    public void Hit()
    {
        animator.SetTrigger("Hit");

        CancelBubble();
    }

    public void Die()
    {
        
        animator.SetBool("isDead", true);
        body.gravityScale = -0.5f;
    }

    public void Respawn()
    {
        // Null check in case this is the first run and player is not yet initialized
        if (animator != null)
        {
            animator.SetBool("isDead", false);
        }
        transform.position = new Vector3(0, -4.5f, 0);
    }
}
