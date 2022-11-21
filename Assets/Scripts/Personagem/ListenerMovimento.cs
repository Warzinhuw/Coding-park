using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenerMovimento : MonoBehaviour {

    private Animator playerAnimator;
    private Rigidbody2D playerRigidBody;
    public Transform contatoChao;

    public bool estaNoChao = false;
    public bool estaOlhandoParaDireita = true;

    public float velocidade;
    public float direcao;

    // Start is called before the first frame update
    void Start() {
        playerAnimator = GetComponent<Animator>();
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        estaNoChao = Physics2D.Linecast(transform.position, contatoChao.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnimator.SetBool("estaNoChao", estaNoChao);

        direcao = Input.GetAxisRaw("Horizontal");
        ExecutaMovimentos();
    }
    void ExecutaMovimentos() {
        playerAnimator.SetBool("estaCorrendo", playerRigidBody.velocity.x != 0f && estaNoChao);
    }

}