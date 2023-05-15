using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot_Player : MonoBehaviour
{
    Shooter shooter;
    // Start is called before the first frame update
    void Start()
    {
        shooter = GetComponent<Shooter>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir_to_player =  GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
        foreach(Shooter.Gun g in shooter.guns_l)
        {
            g.dir = dir_to_player;
        }
    }
}
