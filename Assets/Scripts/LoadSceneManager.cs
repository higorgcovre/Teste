using System.Collections;
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
        LoadScena(scena);
    }

    public void LoadScena(int __scena)
    {
        StartCoroutine(LoadScene(__scena));
    }

    IEnumerator LoadScene(int _scena)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_scena);

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
