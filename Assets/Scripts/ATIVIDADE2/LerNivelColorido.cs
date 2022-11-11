using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LerNivelColorido : MonoBehaviour {
    [Header("Fonte de água")]
    public GameObject fonte;
    public Sprite[] spritesFonte;
    public TextMeshProUGUI inputField;

    [Header("Botão do timer")]
    public TextMeshProUGUI tempoInputField;
    public Button botaoExecutar;

    [Header("Botão colorido")]
    public Button botaoColorido;
    public Color[] coresBotao;

    private Sprite[,] sprites;


    private int indiceCor;
    private float startTimer;


    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        indiceCor = 0;
        int index = 0;
        sprites = new Sprite[spritesFonte.Length, 3];
        Debug.Log("tamanho sprites: " + spritesFonte.Length);
        for (int i = 0; i < spritesFonte.Length / 3; i++) {
            for (int n = 0; n < 3; n++) {
                sprites[i, n] = spritesFonte[index++];
            }
        }
    }

    public void incrementarCor() {
        indiceCor++;
        if (indiceCor > 2) {
            indiceCor = 0;
        }
        RegularFonte();
    }

    public void decrementarCor() {
        indiceCor--;
        if (indiceCor < 0) {
            indiceCor = 2;
        }
        RegularFonte();
    }

    public void incrementarNivelFonte() {
        int estadoAtual = int.Parse(inputField.text);
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
        RegularFonte();
    }

    public void IncrementarTempo() {
        tempoInputField.text = (double.Parse(tempoInputField.text) + 0.5).ToString();
    }

    public void DecrementarTempo() {
        double novoTempo = double.Parse(tempoInputField.text) - 0.5;
        if (novoTempo < 0) {
            return;
        }
        tempoInputField.text = novoTempo.ToString();
    }

    private void SetTimerText(string text) {
        tempoInputField.text = text;
    }

    public void ExecutarScript() {
        float parsedTimer = float.Parse(tempoInputField.text);
        startTimer = 0f;
        while (parsedTimer >= 0) {
            string timerString = parsedTimer.ToString();
            this.Invoke(() => SetTimerText(timerString), startTimer);
            parsedTimer -= 0.5f;
            Invoke(nameof(incrementarCor), startTimer);
            startTimer += 0.5f;
        }
    }


    public void RegularFonte() {

        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();

        // desligado = 0, ligado = 1, cheio = 2
        int estado = int.Parse(inputField.text);
        spriteRenderer.sprite = sprites[indiceCor, estado];
        botaoColorido.image.color = coresBotao[indiceCor];

    }

}

public static class Utility {
    public static void Invoke(this MonoBehaviour mb, Action f, float delay) {
        mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(System.Action f, float delay) {
        yield return new WaitForSeconds(delay);
        f();
    }
}
