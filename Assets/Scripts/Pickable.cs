using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private enum PickUp { Health, Points, Weapon_Upgrade, Weapon_Switch};
    [SerializeField]
    private PickUp pick_up = PickUp.Health;
    [SerializeField]
    private int amount = 1;
    [System.Serializable]
    public struct Upgrade
    {
        public int front;
        public int side;
        public int back;
    }
    [SerializeField]
    private Upgrade upgrade;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch(pick_up)
            {
                case PickUp.Health:
                    collision.GetComponent<Health>().Heal(amount);
                    Destroy(gameObject);
                    break;
                case PickUp.Points:
                    Destroy(gameObject);
                    break;
                case PickUp.Weapon_Upgrade:
                    collision.GetComponent<Weapons_Manager>().AddAttackLevels(upgrade.front, upgrade.side, upgrade.back);
                    Destroy(transform.parent.gameObject);
                    break;
                case PickUp.Weapon_Switch:
                    int new_wep = Random.Range(0, 2);
                    while(new_wep == collision.GetComponent<Weapons_Manager>().GetCurrentWeapon())
                        new_wep = Random.Range(0, 2);
                    collision.GetComponent<Weapons_Manager>().ChangeWeapon(new_wep);
                    Destroy(transform.parent.gameObject);
                    break;
            }
            GameObject.FindGameObjectWithTag("Manager").GetComponent<WaveSpawner>().NextWave();
        }
    }
}
