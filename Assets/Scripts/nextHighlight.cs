using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class nextHighlight : MonoBehaviour
{
    [Header("Objetos para highlight")]
    public GameObject[] highlights;
    public GameObject buttonsIncrDecr;
    public GameObject highlightsObject;
    public GameObject[] highlightsObjectCodigo;


    public GameObject tutorialPanel;
    public GameObject botaoParaDesabilitar1x;


    private int currentHighlight = -1;
    private int currentHighlightForTriggers = -1;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(NextHighlight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentHighlight(int valor) {
        currentHighlight = valor;
        currentHighlightForTriggers = 0;
    }

    void NextHighlight() {
        currentHighlight++;
        currentHighlightForTriggers++;
        if (currentHighlight == 0) {
            if(botaoParaDesabilitar1x)
                botaoParaDesabilitar1x.SetActive(false);
        }
        if (highlights.Length > currentHighlight) {
            if (currentHighlight > 0) {
                highlights[currentHighlight - 1].SetActive(false);
            }
            
            highlights[currentHighlight].SetActive(true);
            
            if (currentHighlight == highlights.Length-1) {
                buttonsIncrDecr.SetActive(true);
                botaoParaDesabilitar1x.SetActive(false);
            }
        }
        if (currentHighlightForTriggers != 0) {
            highlightsObjectCodigo[currentHighlightForTriggers - 1].SetActive(false);
        }
        if (currentHighlightForTriggers < highlightsObjectCodigo.Length - 1)
            highlightsObjectCodigo[currentHighlightForTriggers].SetActive(true);
    }
}
