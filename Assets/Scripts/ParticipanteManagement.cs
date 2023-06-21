using Firebase.Storage;
using Firebase;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class ParticipanteManagement : MonoBehaviour
{
    private int nVotosSim, nVotosNao;
    [SerializeField] private TextMeshProUGUI nVotosSimT, nVotosNaoT;
    [SerializeField] private bool permitidoVotar = true;
    //[SerializeField] private string descricao, nomeVideo;
    [SerializeField] private TextMeshProUGUI textoDescricao, textoNomeVideo, textoNameVideo;
    public VideoPlayer videoPlayer;
    public RawImage rawImage;
    private string path;
    private string caminhoNoBucket = "gs://teste-6010d.appspot.com/";


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
    private IEnumerator UploadVideo(string caminhoLocalDoVideo)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference videoRef = storage.GetReferenceFromUrl(caminhoNoBucket+textoNomeVideo.text);
        print("Linha 0");
        var videoUpTask = videoRef.PutFileAsync(caminhoLocalDoVideo);
        yield return new WaitUntil(predicate: () => videoUpTask.IsCompleted);

        if(videoUpTask.Exception != null)
        {
            print("Ocorreu algum erro: " + videoUpTask.Exception);
        }

        print("Carregou!!");

    }
    void LoadVideoFromFirebaseStorage(string caminhoNoBucket)
    {
        //FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        //StorageReference videoRef = storage.GetReferenceFromUrl(caminhoNoBucket);

        print("Estou aqui no carregamento do vídeo!");
        videoPlayer.url = caminhoNoBucket;
        // Baixe o arquivo de vídeo
        /*videoRef.GetBytesAsync(long.MaxValue).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Falha ao carregar o vídeo: " + task.Exception);
            }
            else
            {
                byte[] videoBytes = task.Result;

                Texture2D videoTexture = new Texture2D(2, 2);
                videoTexture.LoadImage(videoBytes);

                RenderTexture renderTexture = new RenderTexture(videoTexture.width, videoTexture.height, 0);
                renderTexture.Create();
                
                videoPlayer.targetTexture = renderTexture;
                videoPlayer.Play();
          
                Debug.Log("Vídeo carregado com sucesso!");
            }
        });*/
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
    public void EscolherVideo()
    {
        path = EditorUtility.OpenFilePanel("Mostrando todos os vídeos", "", "mp4");

        if (path != null)
        {
            print("O caminho foi salvo");
        }
    }
    public void UploadVideo()
    {
        if (path != null && textoNomeVideo.text != "")
        {
            print("Caminho para o vídeo definido com sucesso!");
        }
        else
        {
            print("Nenhum vídeo encontrado, por favor selecione um video de seu computador!");
        }
    }
    public void UploadVideoEnter()
    {
        if(path != null && textoNomeVideo.text != "")
        {
            print("Video enviado com sucesso!");
            StartCoroutine(UploadVideo(path));
           
        }
        else
        {
            print("Por favor selecione o vídeo e preencha o nome");
        }
       
    }
    public void LoadVideoEnter()
    {
            print("Video com Name");
            LoadVideoFromFirebaseStorage(caminhoNoBucket + textoNameVideo.text);
    }
}
  
