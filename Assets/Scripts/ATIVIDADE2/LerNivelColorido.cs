using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using Image = UnityEngine.UI.Image;

public class Execucao {
    public int Cor { get; }
    public int Nivel { get; }
    public double Tempo { get; }

    public GameObject Prefab { get; set; }
    public TextMeshProUGUI TextNivel { get; set; }
    public TextMeshProUGUI TextTempo { get; set; }
    public Image Image { get; set; }

    public Execucao(int cor, int nivel, double tempo) {
        this.Cor = cor;
        this.Nivel = nivel;
        this.Tempo = tempo;
    }
}

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

    [Header("Botao do Nível")]
    public TextMeshProUGUI nivelInputField;

    [Header("Painel de controle")]
    public GameObject painelReferencia;
    public GameObject prefabExecucao;
    private readonly Color32 fundoInicialPrefab = new(255, 223, 223, 255);
    private readonly Color32 fundoFinalPrefab = new(0, 251, 59, 255);
    public TMP_InputField inputFieldQtdRepeticoes;

    private Sprite[,] sprites;
    private int indiceCor;
    private float startTimer;
    private List<Execucao> execucoes = new List<Execucao>();
    private int qtdRepeticoes = 1;


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

    public void AdicionarExecucao() {
        if (execucoes.Count < 3) {
            Execucao novaExecucao = new Execucao(indiceCor, int.Parse(inputField.text), double.Parse(tempoInputField.text.Split("s")[0]));
            GameObject prefab = Instantiate(prefabExecucao, painelReferencia.transform.position - Vector3.up * 0.4f * (execucoes.Count + 1), Quaternion.identity, painelReferencia.transform.parent.transform);
            prefab.transform.GetChild(0).GetComponent<Image>().color = coresBotao[novaExecucao.Cor];
            prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Nivel.ToString();
            prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Tempo.ToString() + "s";
            novaExecucao.Prefab = prefab;
            novaExecucao.Image = prefab.transform.GetChild(0).GetComponent<Image>();
            novaExecucao.TextNivel = prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            novaExecucao.TextTempo = prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            execucoes.Add(novaExecucao);
            painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (" + execucoes.Count.ToString() + "/3)";
            Debug.Log("execucoesNivel: " + int.Parse(inputField.text) + " execucoesCor: " + indiceCor);
        }
        ResetarFonte();
    }

    private void ResetarFonte() {
        tempoInputField.text = "0";
        inputField.text = "0";
        indiceCor = 0;
        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();
        // desligado = 0, ligado = 1, cheio = 2
        spriteRenderer.sprite = sprites[0, 0];
        botaoColorido.image.color = coresBotao[0];
    }

    public void ExecutarScript() {
        startTimer = 0f;
        for (int n = 0; n < qtdRepeticoes; qtdRepeticoes++) {
            for (int i = 0; i <= execucoes.Count - 1; i++) {
                double parsedTimer = execucoes[i].Tempo;
                int index = i;
                this.Invoke(() => SetPrefabBackground(index, fundoFinalPrefab), startTimer);
                this.Invoke(() => SetNivelText(index), startTimer);
                while (parsedTimer > 0) {
                    string timerString = parsedTimer.ToString();
                    this.Invoke(() => SetTimerText(timerString, index), startTimer);
                    this.Invoke(() => RegularFonteScript(index), startTimer);
                    parsedTimer -= 1f;
                    startTimer += 1f;
                }
                this.Invoke(() => SetPrefabBackground(index, fundoInicialPrefab), startTimer);
                this.Invoke(() => SetTimerText("0", index), startTimer);
            }
            inputFieldQtdRepeticoes.text = (--qtdRepeticoes).ToString();
        }
        this.Invoke(() => ResetarFonte(), startTimer);
        this.Invoke(() => {
            for (int i = 0; i < execucoes.Count; i++) {
                Destroy(execucoes[i].Prefab);
            }
        }, startTimer);
        this.Invoke(() => execucoes.Clear(), startTimer);
        this.Invoke(() => painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (0/3)", startTimer);
    }

    private void SetPrefabBackground(int index, Color32 cor) {
        execucoes[index].Prefab.GetComponent<Image>().color = cor;
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
        tempoInputField.text = (double.Parse(tempoInputField.text.Split("s")[0]) + 1).ToString() + "s";
    }

    public void DecrementarTempo() {
        double novoTempo = double.Parse(tempoInputField.text.Split("s")[0]) - 1;
        if (novoTempo < 0) {
            return;
        }
        tempoInputField.text = novoTempo.ToString() + "s";
    }

    public void SetQtdRepeticoes() {
        qtdRepeticoes = int.Parse(inputFieldQtdRepeticoes.text);
    }

    private void SetTimerText(string text, int index) {
        execucoes[index].TextTempo.text = text + "s";
    }

    private void SetNivelText(int index) {
        execucoes[index].TextNivel.text = execucoes[index].Nivel.ToString();
    }

    private void RegularFonte() {

        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();

        // desligado = 0, ligado = 1, cheio = 2
        int estado = int.Parse(inputField.text);
        spriteRenderer.sprite = sprites[indiceCor, estado];
        botaoColorido.image.color = coresBotao[indiceCor];

    }

    private void RegularFonteScript(int indice) {
        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();
        // desligado = 0, ligado = 1, cheio = 2
        spriteRenderer.sprite = sprites[execucoes[indice].Cor, execucoes[indice].Nivel];
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
