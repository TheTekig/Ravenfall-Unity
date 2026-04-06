using UnityEngine;
using System.Collections.Generic;

public class HitboxController : MonoBehaviour
{
    [SerializeField] private List<GameObject> ObjectsInsideHitbox;
    [SerializeField] private string alvoTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(alvoTag))
        {
            ObjectsInsideHitbox.Add(collision.gameObject);
        }
    }
 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(alvoTag))
        {
            ObjectsInsideHitbox.Remove(collision.gameObject);
        }
    }

    public bool IsObjectInsideHitbox(GameObject obj)
    {
        return ObjectsInsideHitbox.Count > 0;
    }

    public void ApplyDamage(int damage)
    {
        Debug.Log("Objetos dentro do hitbox: " + ObjectsInsideHitbox.Count);
        List<GameObject> targets = new List<GameObject>(ObjectsInsideHitbox);
        foreach (GameObject obj in targets)
        {
            Life life = obj.GetComponent<Life>();
            if (life != null)
            {
                life.SubtractHP(damage);
            }
        }
    }
}
