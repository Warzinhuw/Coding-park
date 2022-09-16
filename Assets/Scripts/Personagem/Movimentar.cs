using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movimentar : MonoBehaviour {

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
        Debug.Log(estaNoChao);
        playerAnimator.SetBool("estaNoChao", estaNoChao);

        direcao = Input.GetAxisRaw("Horizontal");
        ExecutaMovimentos();
    }

    void Flip() {
        estaOlhandoParaDireita = !estaOlhandoParaDireita;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void FixedUpdate() {
        MovePlayer(direcao);
    }

    void ExecutaMovimentos() {
        playerAnimator.SetBool("estaCorrendo", playerRigidBody.velocity.x != 0f && estaNoChao);
    }

    void MovePlayer(float movimentoH) {
        playerRigidBody.velocity = new Vector2(movimentoH * velocidade, playerRigidBody.velocity.y);
        if (movimentoH < 0 && estaOlhandoParaDireita || (movimentoH > 0 && !estaOlhandoParaDireita)) {
            Flip();
        }
    }

}
