using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public bool IsBlowingUp = true; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pop()
    {
        // Play a popping animation
        // Destroy the bubble object
        Destroy(gameObject);
    }
}
