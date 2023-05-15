using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float max_hp = 1, current_hp = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        current_hp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
        //if dead
        if(current_hp <= 0)
        {
            if(gameObject.tag != "Player")
                Destroy(gameObject);
            //else - end run
        }
    }

    public void Damage(float amount)
    {
        if (current_hp - amount >= 0)
            current_hp -= amount;
        else
            current_hp = 0;
    }
    public void Heal(float amount)
    {
        if (current_hp < max_hp)
            if (current_hp + amount > max_hp)
                current_hp = max_hp;
            else
                current_hp += amount;
    }
}
