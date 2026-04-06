using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER: " + other.name);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("COLLISION: " + other.gameObject.name);
    }
}