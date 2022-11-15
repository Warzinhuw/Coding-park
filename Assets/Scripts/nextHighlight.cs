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

    public GameObject tutorialPanel;


    private int currentHighlight = -1;
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

    void NextHighlight() {
        currentHighlight++;
        if (highlights.Length > currentHighlight) {
            if (currentHighlight > 0) {
                highlights[currentHighlight - 1].SetActive(false);
            }
            highlights[currentHighlight].SetActive(true);
            if (currentHighlight == highlights.Length-1) {
                buttonsIncrDecr.SetActive(true);
            }
        } else {
            tutorialPanel.SetActive(false);
            highlightsObject.SetActive(false);
        }
    }
}
