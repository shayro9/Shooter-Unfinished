using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Wave", menuName = "Wave")]
public class Wave : ScriptableObject
{
    [System.Serializable]
    public struct miniWave
    {
        public GameObject mini_wave;
        public float time;
    }

    public miniWave[] mini_waves;
}
