using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 1;
    public bool destroy_onHit = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "projectile_killer")
            if (destroy_onHit)
                Destroy(gameObject);
        //good projectile and enemy
        if (gameObject.layer == 7 && collision.tag == "enemy")
        {
            collision.GetComponent<Health>().Damage(damage);
            if (destroy_onHit)
                Destroy(gameObject);
        }
        //bad projectile and player
        if (gameObject.layer == 8 && collision.tag == "Player")
        {
            collision.GetComponent<Health>().Damage(damage);
            if (destroy_onHit)
                Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameObject.layer == 7 && collision.tag == "enemy" && gameObject.name.StartsWith("Back"))
            collision.GetComponent<Health>().Damage(damage);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
