using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shockwave_Laser : MonoBehaviour
{
    public float scale = 0.1f, time_alive = 5;
    float life_counter = 0;
    public float aperture = 90f;
    public int raycount = 2;
    public float radius = 50, small_radius = 10;


    private void Start()
    {
        Mesh mesh = new Mesh();
        Vector3 origin = Vector3.zero;

        GetComponent<MeshFilter>().mesh = mesh;

        float d = Mathf.Sqrt(radius * radius - small_radius * small_radius);
        float angle = 0;
        float angle_increase = aperture / raycount;

        Vector3[] vertices = new Vector3[raycount*2 + 2];
        int[] triangles = new int[raycount * 6];

        vertices[0] = new Vector3(origin.x,origin.y + small_radius);

        int vertex_index = 1;
        for(int i = 0; i<= raycount;i++)
        {
            float rad_angle = Mathf.Deg2Rad * angle;
            origin = new Vector3(Mathf.Sin(rad_angle) * small_radius, Mathf.Cos(rad_angle) * small_radius);
            Vector3 vertex = new Vector3(
                            Mathf.Sin(rad_angle) * radius,
                            Mathf.Cos(rad_angle) * radius);
            vertices[vertex_index] = vertex;
            if ( i > 0)
            {
                vertices[vertex_index + 1] = origin;
                vertex_index += 2;
            }
            else
                vertex_index += 1;

            angle += angle_increase;
        }

        int triangle_index = 0;
        int another_index = 0;
        for (int i = 0; i < raycount; i++)
        {
            if (another_index == 0)
            {
                triangles[triangle_index] = another_index;
                triangles[triangle_index + 1] = another_index + 1;
                triangles[triangle_index + 2] = another_index + 2;

                triangles[triangle_index + 3] = another_index;
                triangles[triangle_index + 4] = another_index + 2;
                triangles[triangle_index + 5] = another_index + 3;

                another_index += 3;
                triangle_index += 6;
            }
            else
            {
                triangles[triangle_index] = another_index;
                triangles[triangle_index + 1] = another_index - 1;
                triangles[triangle_index + 2] = another_index + 1;

                triangles[triangle_index + 3] = another_index;
                triangles[triangle_index + 4] = another_index + 1;
                triangles[triangle_index + 5] = another_index + 2;

                triangle_index += 6;
                another_index += 2;
            }
        }

        PolygonCollider2D col = GetComponent<PolygonCollider2D>();
        col.pathCount = 1;
        Vector2[] path = new Vector2[vertices.Length];
        for (int i = 0; i< vertices.Length; i++)
        {
            if (i % 2 == 0)
            {
                if (i < 2)
                    path[i / 2] = vertices[i];
                else
                    path[vertices.Length - (i / 2 + 1)] = vertices[i];
            }
            else
            {
                if (i < 2)
                    path[vertices.Length - (i / 2 + 1)] = vertices[i];
                else
                    path[i / 2] = vertices[i];
            }
        }
        col.SetPath(0, path);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }


    void Update()
    {
        transform.localScale += new Vector3(scale, scale);
        if (life_counter > time_alive)
            Destroy(gameObject);
        else
            life_counter += Time.deltaTime;
    }
}
