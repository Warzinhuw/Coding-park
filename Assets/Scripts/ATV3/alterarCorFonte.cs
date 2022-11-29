using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class alterarCorFonte : MonoBehaviour
{

    [Header("Botões")]
    public Button nextColor;
    public Button previousColor;
    public Button coloredButton;

    [Header("Fonte")]
    public Sprite[] spritesFontes;
    public GameObject fonte;

    /* Cores
    AE - 00187B
    AM - 6E8EFF
    AC - 3CABE9
    */

    private static Color azulClaro = new(0.2352941f, 0.6705883f, 0.9137255f, 1f);
    private static Color azulMarinho = new(0.4313726f, 0.5568628f, 1f, 1f);
    private static Color azulEscuro = new(0f, 0.09411765f, 0.4823529f, 1f);

    private readonly Color[] colors = { azulClaro, azulMarinho, azulEscuro};
    private int currentColorIndex = 0;
    private int arraySize;
    // Start is called before the first frame update
    void Start()
    {
        arraySize = colors.Length;
        nextColor.onClick.AddListener(NextColor);
        previousColor.onClick.AddListener(PreviousColor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void NextColor() {
        currentColorIndex++;
        if(currentColorIndex == arraySize) {
            currentColorIndex = 0;
        }
        ColorBlock buttonColors = coloredButton.colors;
        buttonColors.normalColor = colors[currentColorIndex];
        coloredButton.colors = buttonColors;
        fonte.GetComponent<SpriteRenderer>().sprite = spritesFontes[currentColorIndex];
    }

    void PreviousColor() {
        currentColorIndex--;
        if (currentColorIndex < 0) {
            currentColorIndex = arraySize-1;
        }
        ColorBlock buttonColors = coloredButton.colors;
        buttonColors.normalColor = colors[currentColorIndex];
        coloredButton.colors = buttonColors;
        fonte.GetComponent<SpriteRenderer>().sprite = spritesFontes[currentColorIndex];
    }

}
