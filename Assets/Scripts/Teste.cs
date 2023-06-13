using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teste : MonoBehaviour
{
    public GameObject Login;
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadSceneAsync("");
        Login.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
