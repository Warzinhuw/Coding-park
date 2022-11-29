using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodigoFonte : MonoBehaviour
{
    public TMP_InputField inputFieldSlider;
    public Sprite[] sprites;
    public SpriteRenderer srFonte;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementarValorSlider() {
        int valorAtual = int.Parse(inputFieldSlider.text);
        if (valorAtual == 0) {
            inputFieldSlider.text = "1";
        }
    }
    public void DecrementarValorSlider() {
        int valorAtual = int.Parse(inputFieldSlider.text);
        if (valorAtual == 1) {
            inputFieldSlider.text = "0";
        }
    }

    public void ExecutarScript() {
        int valorAtual = int.Parse(inputFieldSlider.text);
        srFonte.sprite = sprites[valorAtual]; 
    }
}
