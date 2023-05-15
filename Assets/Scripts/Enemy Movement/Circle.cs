using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public float radius = 5, move_speed = 5;
    [Range(-1,1)]
    public int dir = 1;
    Vector2 center;
    [Range(0,Mathf.PI * 2)]
    public float angle = Mathf.PI;
    // Start is cal1led before the first frame update
    void Start()
    {
        Vector3 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        center = transform.position + offset;
    }

    // Update is called once per frame
    void Update()
    {
        angle += move_speed * Time.deltaTime * dir;

        Vector2 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = center - offset;
    }
    private void OnDrawGizmosSelected()
    {
        Vector3 offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        Gizmos.DrawWireSphere(transform.position + offset, radius);
    }
}
