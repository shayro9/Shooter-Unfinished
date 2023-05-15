using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public Rigidbody2D projectile;
    public float fire_power = 0, fire_rate = 1;
    float[] fire_rate_counters;
    float fire_rate_counter;
    public int guns = 1;
    public float damage;
    public float guns_offset = 0.1f;


    [System.Serializable]
    public class Gun
    {
        public Rigidbody2D prefab;
        public float damage;
        public float power;
        public float rate;
        public Vector3 local_pos;
        public Vector2 dir;

        public Gun( Rigidbody2D Prefab, float Damage, float Power, float Rate, Vector2 LocalPos, Vector2 Dir)
        {
            prefab = Prefab;
            damage = Damage;
            power = Power;
            rate = Rate;
            local_pos = LocalPos;
            dir = Dir;
        }
    }

    public Gun[] guns_l;

    public enum GunsSetUp { Auto, Manual};
    public GunsSetUp set_up;

    // Start is called before the first frame update
    void Start()
    {
        SetUp(guns);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") != 0 || gameObject.tag == "enemy")
        {
            if (set_up == GunsSetUp.Manual)
            {
                for (int i = 0; i < guns_l.Length; i++)
                {
                    if (fire_rate_counters[i] <= 0)
                    {
                        fire_rate_counters[i] = guns_l[i].rate;
                        Rigidbody2D new_projectile = Instantiate(guns_l[i].prefab, transform.position + guns_l[i].local_pos, Quaternion.identity);
                        new_projectile.GetComponent<Projectile>().damage = guns_l[i].damage;
                        new_projectile.transform.SetParent(GameObject.Find("Projectiles").transform);
                        new_projectile.velocity = guns_l[i].dir * guns_l[i].power;

                        //set projectile facing dir
                        float angle = Mathf.Atan2(guns_l[i].dir.x, guns_l[i].dir.y) * Mathf.Rad2Deg;
                        new_projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                    }
                }
            }
            else//Auto SetUP
            {
                if (fire_rate_counter <= 0)
                {
                    fire_rate_counter = fire_rate;
                    for (int i = 0; i < guns_l.Length; i++)
                    {
                        Rigidbody2D new_projectile = Instantiate(guns_l[i].prefab, transform.position + guns_l[i].local_pos, Quaternion.identity);
                        new_projectile.GetComponent<Projectile>().damage = guns_l[i].damage;
                        new_projectile.transform.SetParent(GameObject.Find("Projectiles").transform);
                        new_projectile.velocity = guns_l[i].dir * guns_l[i].power;

                        //set projectile facing dir
                        float angle = Mathf.Atan2(guns_l[i].dir.x, guns_l[i].dir.y) * Mathf.Rad2Deg;
                        new_projectile.transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
                    }
                }
            }
        }
        if (set_up == GunsSetUp.Manual)
        {
            for (int i = 0; i < guns_l.Length; i++)
                if (fire_rate_counters[i] > 0)
                    fire_rate_counters[i] -= Time.deltaTime;
        }
        else if (fire_rate_counter > 0)
            fire_rate_counter -= Time.deltaTime;
    }

    public void SetUpGunsValues(int index, Gun gun)
    {
        guns_l[index] = gun;
    }

    public void SetUpGunsArray(int size)
    {
        guns_l = new Gun[size];
    }

    public void SetUp(int gun_num)
    {
        Vector3 parent_size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        if (set_up == GunsSetUp.Auto)
        {
            fire_rate_counter = fire_rate;
            guns_l = new Gun[gun_num];

            for (int i = 0; i < gun_num; i++)
            {
                guns_l[i] = (new Gun(projectile, damage, fire_power, fire_rate, new Vector2(), Vector2.up));
                if (gun_num > 1)
                {
                    float temp_x_pos = -parent_size.x / 2 + (parent_size.x / (gun_num - 1)) * i;
                    guns_l[i].local_pos =
                        new Vector2(temp_x_pos,
                        guns_offset + (Mathf.Sign((gun_num / 2) - (i + 1))) * temp_x_pos * parent_size.y / parent_size.x + parent_size.y / 2);
                }
                else
                    guns_l[i].local_pos = new Vector3(0, parent_size.y / 2, 0);
                GameObject temp_gun = new GameObject("gun_" + i);
                temp_gun.transform.SetParent(transform);
                temp_gun.transform.localPosition = guns_l[i].local_pos;
            }
        }
        else//MANUAL SetUP
        {
            fire_rate_counters = new float[guns_l.Length];
            for (int i = 0; i < guns_l.Length; i++)
            {
                GameObject temp_gun = new GameObject("gun_" + i);
                temp_gun.transform.SetParent(transform);
                temp_gun.transform.localPosition = guns_l[i].local_pos;

                fire_rate_counters[i] = guns_l[i].rate;
            }
        }
    }
}
