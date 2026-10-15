using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Summary: Simple 4-direction movement for the villain - reads WASD or arrow keys and drives the Rigidbody 2D with snappy stops.

// Instructions: Setup necessary variables in the inspector.
//               Requires the Rigidbody 2D to be on the same object (Body Type = Kinematic).
//               The rest of this script is fixed - do not edit it.


public class PlayerController : MonoBehaviour {

    public float speed;

    private Rigidbody2D rb2D;
    private Vector2 moveVelocity;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + moveVelocity * Time.fixedDeltaTime);
    }
}
