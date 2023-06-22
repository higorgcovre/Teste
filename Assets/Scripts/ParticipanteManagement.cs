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
    private string caminhoNoBucket = "gs://teste-cambui02.appspot.com";
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
        path = EditorUtility.OpenFilePanel("Mostrando todos os vídeos", "", "mp4");
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
            print("Por favor selecione o vídeo e preencha o nome");
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
        print("Video enviado com sucesso!");
        print("Carregou!!");

    }
    public void LoadVideoEnter()
    {
        if (textoNameVideo.text != "")
        {
            print("Video com Name");
            StartCoroutine(LoadVideoFromFirebaseStorage(caminhoNoBucket));
        }
        else
        {
            print("Porfavor preencha o nome do vídeo!");
        }
    }
    private IEnumerator LoadVideoFromFirebaseStorage(string caminhoNoBucket)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        print(caminhoNoBucket+textoNameVideo.text);
        videoRef = storage.GetReferenceFromUrl(caminhoNoBucket + textoNameVideo.text);
        print(videoRef.ToString());
        var videoDownTask = videoRef.GetDownloadUrlAsync();

        yield return new WaitUntil(predicate: ()=> videoDownTask.IsCompleted);
        
        if (!videoDownTask.IsFaulted && !videoDownTask.IsCanceled)
        {
            string videoUrl = videoDownTask.Result.ToString();
            videoPlayer.url = videoUrl;
            videoPlayer.Play();
        }
        else
        {
            Debug.LogError("Erro ao obter a URL do vídeo: " + videoDownTask.Exception);
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