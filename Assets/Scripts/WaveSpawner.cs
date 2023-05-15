using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;
    public float timer;
    public GameObject current_wave;
    public int wave_index = 0;
    public int mini_wave_index = 0;
    public bool stageOver = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = waves[wave_index].mini_waves[mini_wave_index].time;
        CreateWave();
        SetUpIndexes();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stageOver)
        {
            if (current_wave)
            {
                if (timer <= 0)
                {
                    Destroy(current_wave);
                    GetComponent<ObjectSpawner>().SpawnObject();
                }
                else
                {
                    timer -= Time.deltaTime;
                    if (current_wave.transform.childCount == 0)
                    {
                        Destroy(current_wave);
                        GetComponent<ObjectSpawner>().SpawnObject();
                    }

                }
            }
        }
    }

    public void SetUpIndexes()
    {
        mini_wave_index++;
        if (mini_wave_index >= waves[wave_index].mini_waves.Length)
        {
            mini_wave_index = 0;
            wave_index++;
        }
    }
    public void CreateWave()
    {
        current_wave = 
            Instantiate(waves[wave_index].mini_waves[mini_wave_index].mini_wave, transform.position, Quaternion.identity, transform);
    }
    public void NextWave()
    {
        if (wave_index >= waves.Length)
        {
            stageOver = true;
        }
        if (!stageOver)
        {
            timer = waves[wave_index].mini_waves[mini_wave_index].time;
            CreateWave();
            SetUpIndexes();
        }
    }
    public void NextStage()
    {

    }
}
