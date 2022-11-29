using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AcessarAtividades : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void CarregarAtividade(string nomeAtividade) {
        SceneManager.LoadScene(nomeAtividade);
    }
}
