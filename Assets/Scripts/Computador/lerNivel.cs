using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class lerNivel : MonoBehaviour {
    [Header("Fonte de água")]
    public GameObject fonte;
    public Sprite[] spritesFonte;

    private TMP_InputField inputField;

    // Start is called before the first frame update
    void Start() {
        inputField = gameObject.GetComponent<TMP_InputField>();
        Debug.Log("inputField carrgado: " + inputField.text);
    }

    // Update is called once per frame
    void Update() {

    }

    public void incrementarNivelFonte() {
        int estadoAtual = int.Parse(inputField.text);
        Debug.Log("estado atual: " + estadoAtual);
        int novoEstadoFonte = estadoAtual + 1;
        AtualizarTextoFonte(estadoAtual, novoEstadoFonte);
    }

    public void decrementarNivelFonte() {
        int estadoAtual = int.Parse(inputField.text);
        int novoEstadoFonte = estadoAtual - 1;
        AtualizarTextoFonte(estadoAtual, novoEstadoFonte);
    }

    private void AtualizarTextoFonte(int estadoAtual, int novoEstadoFonte) {
        inputField.text = (novoEstadoFonte <= 2 && novoEstadoFonte >= 0) ? novoEstadoFonte.ToString() : estadoAtual.ToString();
    }

    public void RegularFonte() {

        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();

        // desligado = 0, ligado = 1, cheio = 2
        int estado = int.Parse(inputField.text);
        spriteRenderer.sprite = spritesFonte[estado];

    }

}
