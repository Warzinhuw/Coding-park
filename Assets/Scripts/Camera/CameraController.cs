using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector2 velocidade;
    private Transform playerTransform;

    public GameObject player;
    public float smoothTimeX;

    // Start is called before the first frame update
    void Start() {
        playerTransform = player.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update() {

    }

    void FixedUpdate() {
        float posX = Mathf.SmoothDamp(transform.position.x, playerTransform.position.x, ref velocidade.x, smoothTimeX);

        transform.position = new Vector3(posX, transform.position.y, transform.position.z);
    }
}
