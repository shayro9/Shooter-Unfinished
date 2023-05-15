using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    private enum Object2Spawn { Health, Weapon};
    [SerializeField]
    private Object2Spawn object2_spawn;

    [SerializeField]
    private GameObject health_pref, weapon_pref;

    //change when add more pickable
    public void SpawnObject()
    {
        int[] checker = GameObject.FindGameObjectWithTag("Player").GetComponent<Weapons_Manager>().GetRemainUpgrades();
        int rnd_object = Random.Range(0, 100);

        if (checker[0] == 0 && checker[1] == 0 && checker[2] == 0)
            rnd_object += 50;

        if (rnd_object < 50)
            object2_spawn = Object2Spawn.Weapon;
        else
            object2_spawn = Object2Spawn.Health;

        switch (object2_spawn)
        {
            case Object2Spawn.Health:
                Instantiate(health_pref, Vector3.zero, Quaternion.identity);
                break;
            case Object2Spawn.Weapon:
                GameObject upgrade_holder = Instantiate(weapon_pref, Vector3.zero, Quaternion.identity);
                if (checker[0] == 0)
                    upgrade_holder.transform.GetChild(0).gameObject.SetActive(false);
                if (checker[1] == 0)
                    upgrade_holder.transform.GetChild(1).gameObject.SetActive(false);
                if (checker[2] == 0)
                    upgrade_holder.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
    }

    //change when add more pickable
    public void ChangeObject2Spawn(int index)
    {
        switch(index)
        {
            case 0:
                object2_spawn = Object2Spawn.Health;
                break;
            case 1:
                object2_spawn = Object2Spawn.Weapon;
                break;
        }
    }
}
