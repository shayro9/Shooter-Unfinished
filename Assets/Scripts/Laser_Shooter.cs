using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Shooter : MonoBehaviour
{
    //front laser
    [System.Serializable]
    public class Laser
    {
        public GameObject prefab;
        public float damage;
        public Vector3 local_pos;
        public Vector2 dir;

        public Laser(GameObject Prefab, float Damage, Vector2 LocalPos, Vector2 Dir)
        {
            prefab = Prefab;
            damage = Damage;
            local_pos = LocalPos;
            dir = Dir;
        }
    }

    public Laser[] lasers_l;


    bool[] laser_created;
    Vector2 hit_pos;
    LayerMask mask;
    LayerMask back_mask;
    GameObject[] active_laser;

    //side lasers
    public GameObject side_laser;
    public float side_laser_fire_rate = 1;
    float side_laser_counter = 0;
    public float side_laser_opening = 90;

    //back lasers
    public GameObject back_laser;
    public float back_laser_damage = 1;
    public float back_laser_width = 2;
    GameObject active_back_laser;
    void Start()
    {
        mask = LayerMask.GetMask("Enemy", "Projectile_killer");
        back_mask = LayerMask.GetMask("Projectile_killer");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Fire1") != 0)
        {
            //front laser
            int i = 0;
            foreach (Laser l in lasers_l)
            {
                Rigidbody2D Object_hitted = Physics2D.Raycast(transform.position + l.local_pos, l.dir,20,mask).rigidbody;
                Debug.DrawRay(transform.position + l.local_pos, l.dir * 20, Color.red);

                if (Physics2D.Raycast(transform.position + l.local_pos, l.dir,20, mask))
                {
                    float y_offset = Mathf.Abs(Object_hitted.position.y - gameObject.transform.position.y) 
                        - Object_hitted.GetComponent<Collider2D>().bounds.size.y / 2;
                    float x_offset = y_offset / Mathf.Tan(Mathf.Deg2Rad * Vector3.Angle(l.dir, Vector3.right));

                    hit_pos = new Vector2(
                        gameObject.transform.position.x + l.local_pos.x + x_offset,
                        Object_hitted.position.y - Mathf.Sign(l.dir.y) * Object_hitted.GetComponent<Collider2D>().bounds.size.y / 2);

                    if (!laser_created[i])
                    {
                        laser_created[i] = true;
                        active_laser[i] = Instantiate(l.prefab, hit_pos, Quaternion.identity, gameObject.transform);

                        float beam_length = Mathf.Sqrt(x_offset * x_offset + y_offset * y_offset);
                        active_laser[i].transform.GetChild(0).transform.localScale = new Vector3(1, beam_length);
                        active_laser[i].transform.up = l.dir;
                        active_laser[i].transform.position = hit_pos;
                    }
                    else
                    {
                        float beam_length = Mathf.Sqrt(x_offset * x_offset + y_offset * y_offset);
                        active_laser[i].transform.GetChild(0).transform.localScale = new Vector3(1, beam_length);
                        active_laser[i].transform.up = l.dir;
                        active_laser[i].transform.position = hit_pos;
                    }
                    if (Object_hitted.GetComponent<Health>())
                        Object_hitted.GetComponent<Health>().Damage(l.damage * Time.deltaTime);
                }
                i++;
            }

            //side lasers
            if (side_laser != null)
            {
                if (side_laser_counter > side_laser_fire_rate)
                {
                    side_laser_counter = 0;

                    GameObject left = Instantiate(side_laser, Vector3.zero, Quaternion.identity);
                    GameObject right = Instantiate(side_laser, Vector3.zero, Quaternion.identity);
                    
                    left.transform.SetParent(GameObject.Find("Projectiles").transform);
                    right.transform.SetParent(GameObject.Find("Projectiles").transform);
                    
                    left.transform.position = gameObject.transform.position;
                    right.transform.position = gameObject.transform.position;
                    
                    left.GetComponent<Shockwave_Laser>().scale *= -1;

                    right.GetComponent<Shockwave_Laser>().aperture = side_laser_opening;
                    left.GetComponent<Shockwave_Laser>().aperture = side_laser_opening;

                    left.transform.Rotate(Vector3.forward, -(180 - side_laser_opening) / 2);
                    right.transform.Rotate(Vector3.forward, -(180 - side_laser_opening) / 2);
                }
                else
                    side_laser_counter += Time.deltaTime;
            }

            //back laser
            if (back_laser != null)
            {
                float back_laser_length = transform.position.y 
                    - Physics2D.Raycast(transform.position, Vector2.down, 20, back_mask).transform.position.y;
                if (active_back_laser == null)
                {
                    active_back_laser = Instantiate(back_laser, Vector3.zero, Quaternion.identity);
                    active_back_laser.transform.SetParent(transform);
                    active_back_laser.transform.localPosition = Vector3.zero;
                }
                else
                    active_back_laser.SetActive(true);
                transform.Find(active_back_laser.name).localScale = new Vector3(back_laser_width, back_laser_length);
            }

        }
        if (Input.GetAxis("Fire1") == 0)
        {
            for(int i = 0; i< lasers_l.Length; i++)
            {
                laser_created[i] = false;
                Destroy(active_laser[i]);
            }
            if (active_back_laser != null)
                active_back_laser.SetActive(false);
                
        }
    }

    public void SetUpLaserArray(int size)
    {
        lasers_l = new Laser[size];
        laser_created = new bool[lasers_l.Length];
        active_laser = new GameObject[lasers_l.Length];
    }
    public void SetUpLaserValue(int index, Laser l)
    {
        lasers_l[index] = l;

        laser_created[index] = false;
        GameObject temp_laser = new GameObject("laser_" + index);
        temp_laser.transform.SetParent(transform);
        temp_laser.transform.localPosition = lasers_l[index].local_pos;
    }
    public void SetUpSideLaser(GameObject side, float rate, float openinng)
    {
        side_laser = side;
        side_laser_fire_rate = rate;
        side_laser_opening = openinng;
    }
    public void SetUpBackLaser(GameObject back, float damage, float width)
    {
        back_laser = back;
        back_laser_damage = damage;
        back_laser_width = width;
    }
}
