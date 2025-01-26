using UnityEngine;

public class Rotate : MonoBehaviour
{

    public float speed = 1.0f;
    private float offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.offset = transform.localRotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, offset + speed * Time.time));
    }
}
