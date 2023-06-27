using Firebase.Storage;
using Firebase;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class ParticipanteManagement : MonoBehaviour
{
    private int nVotosSim, nVotosNao;
    [SerializeField] private TextMeshProUGUI nVotosSimT, nVotosNaoT;
    [SerializeField] private bool permitidoVotar = true;
    [SerializeField] private TextMeshProUGUI textoDescricao, textoNomeVideo, textoNameVideo;
    public VideoPlayer videoPlayer;
    private string path;
    private string caminhoNoBucket = "gs://teste-cambui02.appspot.com/";
    private StorageReference videoRef;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                FirebaseStorage storage = FirebaseStorage.DefaultInstance;
            }
            else
            {
                Debug.LogError("Falha ao inicializar o Firebase");
            }
        });
    }
    public void EscolherVideo()
    {
#if UNITY_EDITOR
        path = EditorUtility.OpenFilePanel("Mostrando todos os v�deos", "", "mp4");
#endif        
        if (path != null)
        {
            print("O caminho foi salvo");
        }
    }
    public void UploadVideoEnter()
    {
        if(path != null && textoNomeVideo.text != "")
        {
            StartCoroutine(UploadVideo(path));
        }
        else
        {
            print("Por favor selecione o v�deo e preencha o nome");
        }
       
    }
    private IEnumerator UploadVideo(string caminhoLocalDoVideo)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        videoRef = storage.GetReferenceFromUrl(caminhoNoBucket + textoNomeVideo.text);
        var videoUpTask = videoRef.PutFileAsync(caminhoLocalDoVideo);
        yield return new WaitUntil(predicate: () => videoUpTask.IsCompleted);

        if(videoUpTask.Exception != null)
        {
            print("Ocorreu algum erro: " + videoUpTask.Exception);
        }

        Menu_Bancada.instance.AddVideoRecents(textoNomeVideo.text, caminhoNoBucket + textoNomeVideo.text);

        print("Video enviado com sucesso!");
        print("Carregou!!");

    }
    public void DownLoadVideoEnter()
    {
        StartCoroutine(DownLoadVideoFromFirebaseStorage(name));
       
    }
    private IEnumerator DownLoadVideoFromFirebaseStorage(string name)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        string a = caminhoNoBucket + name;
        videoRef = storage.GetReferenceFromUrl(a);
        var videoDownTask = videoRef.GetDownloadUrlAsync();

        yield return new WaitUntil(predicate: ()=> videoDownTask.IsCompleted);
        
        if (!videoDownTask.IsFaulted && !videoDownTask.IsCanceled)
        {
            string videoUrl = videoDownTask.Result.ToString();
            //videoPlayer.url = videoUrl;
            //videoPlayer.Play();
            //textoNameVideo.text = name;
            FindObjectOfType<Menu_Bancada>().OpenVideo(videoUrl);
        }
        else
        {
            Debug.LogError("Erro ao obter a URL do v�deo: " + videoDownTask.Exception);
        }
    }
    public void votarSim()
    {
        if (permitidoVotar)
        {
            nVotosSim++;
            nVotosSimT.text = nVotosSim.ToString();
        }
        permitidoVotar = !permitidoVotar;

    }
    public void votarNao()
    {
        if (permitidoVotar)
        {
            nVotosNao++;
            nVotosNaoT.text = nVotosNao.ToString();
        }
        permitidoVotar = !permitidoVotar;
    }    
}