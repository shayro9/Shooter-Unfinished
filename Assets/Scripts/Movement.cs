using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed = 10;
    Vector2 move = new Vector2(0,0);
    // Update is called once per frame
    void Update()
    {
        Vector2 user_input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (user_input.x != 0)
            move.x = speed * user_input.x;
        else
            move.x = 0;
        if (user_input.y != 0)
            move.y = speed * user_input.y;
        else
            move.y = 0;
        gameObject.GetComponent<Rigidbody2D>().velocity = move;
    }
}
