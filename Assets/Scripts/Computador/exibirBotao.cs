using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class exibirBotao : MonoBehaviour
{
    // Start is called before the first frame update
    public Button botao;
    private GameObject gameObjectBotao;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        gameObjectBotao = botao.gameObject;
        if (collision.gameObject.tag == "Player") {
            gameObjectBotao.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            gameObjectBotao.SetActive(false);
        }
    }

}
