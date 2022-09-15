using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andar : MonoBehaviour {

    private Rigidbody2D rigidBody;

    private float horizontal;

    [SerializeField]
    private float movementSpeed = 10;

    private bool facingRight = true;

    // Start is called before the first frame update
    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        horizontal = Input.GetAxis("Horizontal");
        ///Debug.Log("Função Update: " + Time.deltaTime);

    }

    private void FixedUpdate() {
        HandMovement(horizontal);
        Flip(horizontal);
        //Debug.Log("Função FixedUpdate: " + Time.deltaTime);
    }

    void HandMovement(float horizontal) {
        //rigidBody.velocity = Vector2.left;
        rigidBody.velocity = new Vector2(horizontal * movementSpeed, rigidBody.velocity.y);
        //Debug.Log(rigidBody.velocity);
    }

    void Flip(float horizontal) {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
            facingRight = !facingRight;
            Vector2 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

}
