using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class lerNivel : MonoBehaviour
{
    [Header("Fonte de água")]
    public GameObject fonte;
    public Sprite[] spritesFonte;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<TMP_InputField>().onEndEdit.AddListener(RegularFonte);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void RegularFonte(string estado) {

        SpriteRenderer spriteRenderer = fonte.GetComponent<SpriteRenderer>();

        // Variável que armazena o estado da fonte, pode ser qualquer número inteiro (números sem ",")
        int estadoFonte = int.Parse(estado);

        // desligado = 0, ligado = 1
        if (estadoFonte == 0) {
            spriteRenderer.sprite = spritesFonte[0];
        }
        else if (estadoFonte == 1) {
            spriteRenderer.sprite = spritesFonte[1];
        }
        else {
            Debug.Log("VALOR INVÁLIDO!");
        }

    }

}
