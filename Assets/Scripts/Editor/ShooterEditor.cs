using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Shooter))]
[CanEditMultipleObjects]
public class ShooterEditor : Editor
{
    /*
    private SerializedObject m_Object;
    void OnEnable()
    {
        m_Object = new SerializedObject(target);
    }

    public override void OnInspectorGUI()
    {
        Shooter shooter = (Shooter)target;

        shooter.set_up = (Shooter.GunsSetUp)EditorGUILayout.EnumPopup("Set up Option", shooter.set_up);
        shooter.guns_offset = EditorGUILayout.FloatField("Gun's offset", shooter.guns_offset);
        switch (shooter.set_up)
        {
            case Shooter.GunsSetUp.Auto:
                shooter.guns = EditorGUILayout.IntField("Guns", shooter.guns);
                shooter.projectile = (Rigidbody2D)EditorGUILayout.ObjectField("Projectile Prefab", shooter.projectile, typeof(Rigidbody2D), false);
                shooter.damage = EditorGUILayout.FloatField("Damgae", shooter.damage);
                shooter.fire_power = EditorGUILayout.FloatField("Power", shooter.fire_power);
                shooter.fire_rate = EditorGUILayout.FloatField("Fire Rate", shooter.fire_rate);
                break;
            case Shooter.GunsSetUp.Manual:
                EditorGUILayout.PropertyField(m_Object.FindProperty("guns_l"), new GUIContent("Guns"), true);
                break;

        }
    }*/
}
