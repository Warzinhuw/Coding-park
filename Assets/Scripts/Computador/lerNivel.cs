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
    }

    // Update is called once per frame
    void Update() {

    }

    public void incrementarNivelFonte() {
        int novoEstadoFonte = int.Parse(inputField.text) + 1;
        inputField.text = novoEstadoFonte.ToString();
    }

    public void decrementarNivelFonte() {
        int novoEstadoFonte = int.Parse(inputField.text) - 1;
        inputField.text = novoEstadoFonte.ToString();
    }


    public void RegularFonte() {

        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();

        // desligado = 0, ligado = 1, cheio = 2
        int estado = int.Parse(inputField.text);
        if (estado == 0) {
            spriteRenderer.sprite = spritesFonte[0];
        }
        else if (estado == 1) {
            spriteRenderer.sprite = spritesFonte[1];
        }
        else if (estado == 2) {
            spriteRenderer.sprite = spritesFonte[2];
        }
        else {
            Debug.Log("VALOR INVÁLIDO!");
        }

    }

}
