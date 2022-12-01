using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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
    //3CABE9 6E8EFF 00187B

    [Header("Botao do Nível")]
    public TextMeshProUGUI nivelInputField;

    [Header("Painel de controle")]
    public GameObject painelReferencia;
    public GameObject prefabExecucao;
    private readonly Color32 fundoInicialPrefab = new(255, 223, 223, 255);
    private readonly Color32 fundoFinalPrefab = new(0, 251, 59, 255);
    public TMP_InputField inputFieldQtdRepeticoes;
    public TMP_InputField textoExibirQtdRepeticoes;
    public RectTransform painelTutorial;

    [Header("Botoões para desabilitar")]
    public GameObject botoesParaDesabilitar;
    public GameObject botoesParaDesabilitarConsole;
    public GameObject painelBotoesAtv2;
    public GameObject botaoLimparExecs;

    [Header("Configurações do console")]
    public TMP_InputField inputFieldFuncao1;
    public TMP_InputField inputFieldFuncao2;
    public TMP_InputField inputQtdRepeticoesPeloConsole;
    public GameObject telaConsoleParaDesabilitar;
    public List<GameObject> highLightsToTrigger = new();

    [Header("Camera")]
    public Camera cameraToZoomIn;
    public GameObject canvasWithZoomedCam; 
    public GameObject painelControleExecs;
    public GameObject mainUI;

    private Sprite[,] sprites;
    private int indiceCor;
    private float startTimer;
    private List<Execucao> execucoes = new();
    private int qtdRepeticoes = 1;

    private Regex regexFuncao = new Regex("^alteraFonte\\([1-5]\\s*,\\s*[0-2]+,\\s*[0-2]+\\);$");
    private Color32 fundoMatchRegex = new(83, 255, 113, 255);
    private Color32 fundoNotMatchRegex = new(255, 72, 37, 255);
    private Color32 fundoBranco = new(255, 255, 255, 255);
    private Execucao execucaoDoInput1;
    private Execucao execucaoDoInput2;

    private bool shouldClearExecsInTheEnd = false;


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
        for (int i = 0; i < spritesFonte.Length / 3; i++) {
            for (int n = 0; n < 3; n++) {
                sprites[i, n] = spritesFonte[index++];
            }
        }
    }

    public void AdicionarECriarExecucao() {
        Execucao novaExecucao = MontarNovaExecucao(null);
        MontarExecucoes(novaExecucao);
    }

    public void AdicionarECriarExecucaoComTarget(TMP_InputField targetInputField) {
        Execucao novaExecucao = MontarNovaExecucao(targetInputField);
        MontarExecucoes(novaExecucao);
    }

    public void MontarExecucoes(Execucao novaExecucao) {
        if (execucoes.Count < 3) {
            if (novaExecucao == null)
                return;
            GameObject prefab = Instantiate(prefabExecucao, painelReferencia.transform.position - Vector3.up * 0.66f * (execucoes.Count + 1), Quaternion.identity, painelReferencia.transform.parent.transform);
            prefab.transform.GetChild(0).GetComponent<Image>().color = coresBotao[novaExecucao.Cor];
            prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Nivel.ToString();
            prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Tempo.ToString() + "s";
            novaExecucao.Prefab = prefab;
            novaExecucao.Image = prefab.transform.GetChild(0).GetComponent<Image>();
            novaExecucao.TextNivel = prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            novaExecucao.TextTempo = prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            execucoes.Add(novaExecucao);
            painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (" + execucoes.Count.ToString() + "/3)";
        }
        ResetarFonte();
    }


    private Execucao MontarNovaExecucao(TMP_InputField targetInputField) {
        if(targetInputField == null) {
            return new Execucao(indiceCor, int.Parse(inputField.text), double.Parse(tempoInputField.text.Split("s")[0]));
        } else {
            if (regexFuncao.IsMatch(targetInputField.text)) {
                string[] valores = targetInputField.text.Split(",");
                Execucao novaExecucao = new Execucao(int.Parse(valores[2].Split(")")[0]), int.Parse(valores[1]), int.Parse(valores[0].Split("(")[1]));
                int indice;
                if (targetInputField.name.Contains("1")) {
                    indice = 1;
                }
                else {
                    indice = 2;
                }
                if (indice == 1 && execucaoDoInput1 != null || indice == 2 && execucaoDoInput2 != null) {
                    Execucao execucao = execucoes.ElementAt(indice);
                    if (execucao != null) {
                        targetInputField.image.color = Color32.Lerp(fundoBranco, fundoNotMatchRegex, 1f);
                        this.Invoke(() => targetInputField.image.color = Color32.Lerp(fundoNotMatchRegex, fundoBranco, 1f), 1f);
                        return null;
                    }
                }
                if (indice == 1) {
                    execucaoDoInput1 = novaExecucao;
                } else {
                    execucaoDoInput2 = novaExecucao;
                }
                targetInputField.image.color = Color32.Lerp(fundoBranco, fundoMatchRegex, 1f);
                this.Invoke(() => targetInputField.image.color = Color32.Lerp(fundoNotMatchRegex, fundoBranco, 1f), 1f);
                return novaExecucao;
            } else {
                targetInputField.image.color = Color32.Lerp(fundoBranco, fundoNotMatchRegex, 1f);
                this.Invoke(() => targetInputField.image.color = Color32.Lerp(fundoNotMatchRegex, fundoBranco, 1f), 1f);
                Debug.Log("Regex didn't match for following text: " + targetInputField.text);
                return null;
            }
        }
    }

    private void ResetarFonte() {
        tempoInputField.text = "1s";
        inputField.text = "0";
        indiceCor = 0;
        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();
        // desligado = 0, ligado = 1, cheio = 2
        spriteRenderer.sprite = sprites[0, 0];
        botaoColorido.image.color = coresBotao[0];
        botoesParaDesabilitar.SetActive(true);
        botaoLimparExecs.SetActive(true);
    }
    
    private void ResetPainelControleTamanho() {
        painelControleExecs.GetComponent<RectTransform>().pivot = cameraToZoomIn.isActiveAndEnabled ? new(1, 0) : new(1, 1);
        painelControleExecs.GetComponent<RectTransform>().anchorMin = cameraToZoomIn.isActiveAndEnabled ? new(1, 0) : new(1, 1);
        painelControleExecs.GetComponent<RectTransform>().anchorMax = cameraToZoomIn.isActiveAndEnabled ? new(1, 0) : new(1, 1);
        Vector2 newSize = cameraToZoomIn.isActiveAndEnabled ? new Vector2(0, 0) : new Vector2(-15, -40);
        painelControleExecs.GetComponent<RectTransform>().offsetMin = newSize;
        painelControleExecs.GetComponent<RectTransform>().offsetMax = newSize;
        painelControleExecs.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 437.56f);
        painelControleExecs.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 338.61f);
    }

    public void ExecutarScript() {
        Camera mainCam = Camera.main;
        if (painelBotoesAtv2.activeSelf) {
            if (execucoes.Count == 0 ) {
                AdicionarECriarExecucao();
                shouldClearExecsInTheEnd = true;
            }
            cameraToZoomIn.gameObject.SetActive(true);
            cameraToZoomIn.enabled = true;
            mainCam.enabled = false;
            painelControleExecs.transform.SetParent(canvasWithZoomedCam.transform);
            ResetPainelControleTamanho();
        } else {
            botoesParaDesabilitarConsole.SetActive(false);
        }
        botaoLimparExecs.SetActive(false);
        startTimer = 0.5f;
        int loopCounter = 0;
        textoExibirQtdRepeticoes.text = "Contador: " + qtdRepeticoes;
              
        while (qtdRepeticoes > loopCounter) {
            for (int i = 0; i < execucoes.Count; i++) {
                double parsedTimer = execucoes[i].Tempo;
                int index = i;
                this.Invoke(() => SetPrefabBackground(index, fundoFinalPrefab), startTimer);
                this.Invoke(() => SetNivelText(index), startTimer);
                if(telaConsoleParaDesabilitar.activeSelf) {
                    this.Invoke(() => highLightsToTrigger[index].SetActive(true), startTimer);
                }
                while (parsedTimer >= 0) {
                    string timerString = parsedTimer.ToString();
                    if (parsedTimer != 0) {
                        this.Invoke(() => SetTimerText(timerString, index), startTimer);
                        this.Invoke(() => RegularFonteScript(index), startTimer);
                        startTimer += 1f;
                    } else {
                        this.Invoke(() => SetTimerText("0", index), startTimer);
                        startTimer += 0.5f;
                    }
                    parsedTimer -= 1f;
                }
                this.Invoke(() => SetPrefabBackground(index, fundoInicialPrefab), startTimer);
                this.Invoke(() => highLightsToTrigger[index].SetActive(false), startTimer);
            }
            loopCounter++;
            string repeticoes = (qtdRepeticoes - loopCounter).ToString();
            this.Invoke(() => {
                textoExibirQtdRepeticoes.text = "Contador: " + repeticoes;
                ResetControlPanel();
            }, startTimer);
            
        }
        /*
        this.Invoke(() => {
            for (int i = 0; i < execucoes.Count; i++) {
                Destroy(execucoes[i].Prefab);
            }
        }, startTimer);
        this.Invoke(() => ResetarFonte(), startTimer);
        this.Invoke(() => textoExibirQtdRepeticoes.text = "", startTimer);
        this.Invoke(() => inputFieldQtdRepeticoes.text = "", startTimer);
        this.Invoke(() => painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (0/3)", startTimer);
        this.Invoke(() => execucoes.Clear(), startTimer);
        */
        this.Invoke(() => painelControleExecs.transform.SetParent(mainUI.transform), startTimer + 0.5f);
        if (painelBotoesAtv2.activeSelf) {
            this.Invoke(() => mainCam.enabled = true, startTimer + 0.5f);
            this.Invoke(() => cameraToZoomIn.enabled = false, startTimer + 0.5f);
            this.Invoke(() => ResetPainelControleTamanho(), startTimer + 0.5f);
        } else {
            botoesParaDesabilitarConsole.SetActive(true);
        }

        this.Invoke(() => botaoLimparExecs.SetActive(true), startTimer + 0.5f);

        if (shouldClearExecsInTheEnd) {
            this.Invoke(() => ClearExecucoesSemConsole(), startTimer + 0.5f);
        }
    }

    // Parte console

    public void ClearExecucoesSemConsole() {
        for (int i = 0; i < execucoes.Count; i++) {
            Destroy(execucoes[i].Prefab);
        }
        if (painelBotoesAtv2.activeSelf) {
            ResetarFonte();
            textoExibirQtdRepeticoes.text = "";
            inputFieldQtdRepeticoes.text = "";
            painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (0/3)";
            execucoes.Clear();
        }
        else {
            execucoes.Clear();
            Execucao novaExecucao = new Execucao(0, 1, 3);
            GameObject prefab = Instantiate(prefabExecucao, painelReferencia.transform.position - Vector3.up * 0.66f, Quaternion.identity, painelReferencia.transform.parent.transform);
            prefab.transform.GetChild(0).GetComponent<Image>().color = coresBotao[novaExecucao.Cor];
            prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Nivel.ToString();
            prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = novaExecucao.Tempo.ToString() + "s";
            novaExecucao.Prefab = prefab;
            novaExecucao.Image = prefab.transform.GetChild(0).GetComponent<Image>();
            novaExecucao.TextNivel = prefab.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
            novaExecucao.TextTempo = prefab.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
            execucoes.Insert(0, novaExecucao);
            painelReferencia.GetComponentInChildren<TextMeshProUGUI>().text = "Instruções (" + execucoes.Count.ToString() + "/3)";
            inputFieldFuncao1.text = "";
            inputFieldFuncao2.text = "";
            qtdRepeticoes = 0;
            inputQtdRepeticoesPeloConsole.text = "";
        }
    }

    public void ExecutarPeloConsole() {
        if (execucoes.Count != 3) {
            if (!regexFuncao.IsMatch(inputFieldFuncao1.text)) {
                inputFieldFuncao1.image.color = Color32.Lerp(fundoBranco, fundoNotMatchRegex, 1f);
            } else {
                inputFieldFuncao2.image.color = Color32.Lerp(fundoBranco, fundoNotMatchRegex, 1f);
            }
            return;
        }
        ExecutarScript();
        //this.Invoke(() => ClearExecucoes(), startTimer);
        this.Invoke(() => telaConsoleParaDesabilitar.SetActive(true), startTimer);
    }

    private void ResetControlPanel() {
        for (int i = 0; i < execucoes.Count; i++) {
            execucoes[i].TextTempo.text = execucoes[i].Tempo.ToString() + "s";
            execucoes[i].TextNivel.text = execucoes[i].Nivel.ToString();
        }
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
        if (novoTempo <= 0) {
            return;
        }
        tempoInputField.text = novoTempo.ToString() + "s";
    }

    public void IncrementarQtdReps() {
        try {
            qtdRepeticoes = int.Parse(inputFieldQtdRepeticoes.text);
        } catch {
            qtdRepeticoes = 0;
        }
        qtdRepeticoes++;
        inputFieldQtdRepeticoes.text = qtdRepeticoes.ToString();
    }

    public void DecrementarQtdReps() {
        try {
            qtdRepeticoes = int.Parse(inputFieldQtdRepeticoes.text);
        }
        catch {
            qtdRepeticoes = 0;
        }
        if (qtdRepeticoes - 1 >= 1) {
            qtdRepeticoes--;
            inputFieldQtdRepeticoes.text = qtdRepeticoes.ToString();
        }
    }

    public void SetQtdRepeticoes() {
        qtdRepeticoes = int.Parse(inputFieldQtdRepeticoes.text);
    }

    public void SetQtdRepeticoes1PeloConsole() {
        qtdRepeticoes = int.Parse(inputQtdRepeticoesPeloConsole.text);
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

    public void SetAsFirstSibling() {
        painelTutorial.SetAsFirstSibling();
    }

    public void SetAsLastSibling() {
        painelTutorial.SetAsLastSibling();
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
