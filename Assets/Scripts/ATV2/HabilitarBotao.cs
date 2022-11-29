using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HabilitarBotao : MonoBehaviour
{
    public Slider slider;
    public Sprite[] spritesFonte;
    public GameObject fonte;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AlterarFonte() {
        if (slider.value == 0) {
            fonte.GetComponent<SpriteRenderer>().sprite = spritesFonte[0];
        } else {
            fonte.GetComponent<SpriteRenderer>().sprite = spritesFonte[1];
        }
    }
}
