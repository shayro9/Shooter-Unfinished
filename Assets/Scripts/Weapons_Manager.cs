using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons_Manager : MonoBehaviour
{
    //Level of each attack
    [SerializeField]
    private int front = 1, side = 0, back = 0;
    //max level for each attack
    private int max_front = 4, max_side = 3, max_back = 3;

    //Active weapon type, defult is Gun
    //wep indexes:
    //0-Gun
    //1-Laser
    [SerializeField]
    private int wep = 0;
    [SerializeField]
    private Rigidbody2D front_gun, strong_front_gun, side_gun, back_gun;
    [SerializeField]
    private GameObject front_laser, strong_front_laser, side_laser, back_laser;

    int front_guns = 0, side_guns = 0, back_guns = 0;
    Vector3 parent_size;

    // Start is called before the first frame update
    void Start()
    {
        parent_size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        SetUpWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWeapon(int wep_index)
    {
        wep = wep_index;
        GetComponent<Shooter>().enabled = false;
        GetComponent<Laser_Shooter>().enabled = false;
        SetUpWeapon();
    }
    public int GetCurrentWeapon()
    {
        return wep;
    }
    public void UpdateAttackLevels(int f,int s,int b)
    {
        front = f;
        side = s;
        back = b;
        SetUpWeapon();
    }
    public void AddAttackLevels(int f, int s, int b)
    {
        front += f;
        side += s;
        back += b;
        SetUpWeapon();
    }
    public int[] GetRemainUpgrades()
    {
        return new int[] { max_front - front, max_side - side, max_back - back };
    }

    //adds weapon components depanding on num of guns
    private void SetUpWeapon()
    {
        switch(wep)
        {
            //Gun - Set up the Shooter component
            case 0:
                Shooter temp_component = GetComponent<Shooter>();
                temp_component.enabled = true;
                temp_component.set_up = Shooter.GunsSetUp.Manual;
                setUpNumOfGuns();
                temp_component.SetUpGunsArray(front_guns + side_guns + back_guns);

                float d = 1;
                float p = temp_component.fire_power;
                float r = temp_component.fire_rate;

                //set up front guns
                for (int i = 0; i<front_guns; i++)
                {
                    Vector2 l_p = FrontPosition(front_guns, i);
                    Vector2 dir = Vector2.up;
                    Rigidbody2D pref = front_gun;
                    if (front == 4 && i == 2)
                    {
                        pref = strong_front_gun;
                        d = 3;
                    }

                    Shooter.Gun temp_gun = new Shooter.Gun(pref, d, p, r, l_p, dir);
                    temp_component.SetUpGunsValues(i, temp_gun);
                }
                //set up side guns
                for(int i = 0; i < side_guns; i+=2)
                {
                    Vector3 parent_size = gameObject.GetComponent<SpriteRenderer>().bounds.size;
                    Vector2 pos1 = new Vector2(parent_size.x / 2, 0);
                    Vector2 pos2 = new Vector2(-parent_size.x / 2, 0);
                    Vector2 dir1 = new Vector2();
                    Vector2 dir2 = new Vector2();
                    int temp_deg = 0;
                    switch (side_guns)
                    {
                        case 2:
                            temp_deg = 30;
                            break;
                        case 4:
                            if (i > 0)
                            {
                                temp_deg = 45;
                            }
                            break;
                        case 6:
                            if (i > 2)
                                temp_deg = 45;
                            else if (i > 0)
                                temp_deg = 0;
                            else
                                temp_deg = -30;
                            break;
                    }
                    dir1 = SideDirs(temp_deg);
                    dir2 = SideDirs(temp_deg);
                    dir2 = new Vector2(-dir2.x, dir2.y);

                    Shooter.Gun temp_gun1 = new Shooter.Gun(side_gun, d, p, r, pos1, dir1);
                    Shooter.Gun temp_gun2 = new Shooter.Gun(side_gun, d, p, r, pos2, dir2);

                    temp_component.SetUpGunsValues(i + front_guns, temp_gun1);
                    temp_component.SetUpGunsValues(i + front_guns + 1, temp_gun2);
                }
                //set up back guns
                for(int i = 0; i<back_guns;i++)
                {
                    Vector2 pos ,dir;
                    Vector2[] temp_a = BackPosDir(i, back);
                    pos = temp_a[0];
                    dir = temp_a[1];

                    Shooter.Gun temp_b_gun = new Shooter.Gun(back_gun, d, p, r, pos, dir);

                    temp_component.SetUpGunsValues(i + front_guns + side_guns, temp_b_gun);
                }

                temp_component.SetUp(front_guns + side_guns + back_guns);
                break;
            case 1:
                Laser_Shooter laser_component = GetComponent<Laser_Shooter>();
                laser_component.enabled = true;
                setUpNumOfGuns();
                laser_component.SetUpLaserArray(front_guns);

                float dmg = 1;

                //set up front lasers
                for(int i = 0; i<front_guns; i++)
                {
                    Vector2 lsr_p = FrontPosition(front_guns, i);
                    Vector2 dir = Vector2.up;
                    GameObject pref = front_laser;
                    if (front == 4 && i == 2)
                    {
                        pref = strong_front_laser;
                        dmg = 3;
                    }
                    Laser_Shooter.Laser temp_laser = new Laser_Shooter.Laser(pref, dmg, lsr_p, dir);
                    laser_component.SetUpLaserValue(i, temp_laser);
                }
                //set up side lasers
                float[] temp_rate_open = SideRateOpenning(side);
                laser_component.SetUpSideLaser(side_laser, temp_rate_open[0], temp_rate_open[1]);
                //set up back lasers
                float[] temp_damage_width = BackDamageWidth(back);
                laser_component.SetUpBackLaser(back_laser, temp_damage_width[0], temp_damage_width[1]);
                break;
        }
    }

    //set up the guns pos on player
    private Vector2 FrontPosition(int num,int i)
    {
        if (num > 1)
        {
            float temp_x_pos = -parent_size.x / 2 + (parent_size.x / (num - 1)) * i;
            return new Vector2
            {
                x = temp_x_pos,
                y = 0.1f + (Mathf.Sign((num / 2) - (i + 1))) * temp_x_pos * parent_size.y / parent_size.x + parent_size.y / 2
            };
        }
        return new Vector3(0, parent_size.y / 2, 0);
    }

    //returns the dir for each deg
    private Vector2 SideDirs(int deg)
    {
        Vector2 dir = new Vector2();

        dir = new Vector2
        {
            x = Mathf.Cos(deg * Mathf.Deg2Rad),
            y = Mathf.Sin(deg * Mathf.Deg2Rad)
        };

        return dir;
    }

    //returns for each index the pos on player and dir to shoot back
    private Vector2[] BackPosDir(int index,int level)
    {
        Vector2 pos = new Vector2(), dir = new Vector2();
        int deg = 0;
        switch (level)
        {
            case 1:
                pos = new Vector2(parent_size.x / 4, 0);
                deg = 270 + 20;//deg from down is 270 + (-deg)
                break;
            case 2:
                if (index > 1)
                {
                    pos = new Vector2(parent_size.x / 4, 0);
                    deg = 270 + 20;
                }
                else
                {
                    pos = new Vector2(parent_size.x / 2, 0);
                    deg = 270 + 35;
                }
                break;
            case 3:
                if (index > 5)
                {
                    pos = new Vector2(0, 0);
                    deg = 270;
                }
                else if (index > 3)
                {
                    pos = new Vector2(parent_size.x / 6, 0);
                    deg = 270 + 20;
                }
                else if (index > 1)
                {
                    pos = new Vector2(parent_size.x / 3, 0);
                    deg = 270 + 35;
                }
                else
                {
                    pos = new Vector2(parent_size.x / 2, 0);
                    deg = 270 + 50;
                }
                break;
        }
        dir = new Vector2
        {
            x = Mathf.Cos(deg * Mathf.Deg2Rad),
            y = Mathf.Sin(deg * Mathf.Deg2Rad)
        };

        if (index % 2 == 0)
            return new Vector2[] { pos, dir };
        return new Vector2[] { new Vector2(-pos.x, 0),new Vector2(-dir.x,dir.y) };
    }

    //set up number of guns for each gun level
    public void setUpNumOfGuns()
    {
        switch (front)
        {
            case 1:
                front_guns = 1;
                break;
            case 2:
                front_guns = 3;
                break;
            case 3:
                front_guns = 5;
                break;
            case 4:
                front_guns = 5;
                break;
        }
        switch (side)
        {
            case 0:
                side_guns = 0;
                break;
            case 1:
                side_guns = 2;
                break;
            case 2:
                side_guns = 4;
                break;
            case 3:
                side_guns = 6;
                break;
        }
        switch (back)
        {
            case 0:
                back_guns = 0;
                break;
            case 1:
                back_guns = 2;
                break;
            case 2:
                back_guns = 4;
                break;
            case 3:
                back_guns = 7;
                break;
        }
    }

    private float[] SideRateOpenning(int level)
    {
        switch(level)
        {
            case 1:
                return new float[] { 0.5f, 90 };
            case 2:
                return new float[] { 0.2f, 90 };
            case 3:
                return new float[] { 0.5f, 120 };
            default:
                return new float[] { 0, 0 };
        }
    }
    private float[] BackDamageWidth(int level)
    {
        switch (level)
        {
            case 1:
                return new float[] { 2, 1.5f };
            case 2:
                return new float[] { 2, 2 };
            case 3:
                return new float[] { 3, 4};
            default:
                return new float[] { 0, 0 };
        }
    }
}
