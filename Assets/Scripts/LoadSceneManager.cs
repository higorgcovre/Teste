using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager instance;
    public static int scena;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    private void Start()
    {
        instance = this;
    }

    public void Load(int scena)
    {
        SceneManager.LoadScene(2);
        LoadSceneManager.scena = scena;
        LoadScena();
    }

    public void LoadScena()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
                    yield return new WaitForSeconds(1f);
            }
            yield return null;
        }

        print("Scena Carregada");
    }
}
