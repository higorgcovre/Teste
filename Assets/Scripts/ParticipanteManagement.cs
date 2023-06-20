using Firebase.Storage;
using Firebase;
using System.Threading.Tasks;
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
    [SerializeField] private string descricao, nomeVideo;
    [SerializeField] private TextMeshProUGUI textoDescricao, textoNomeVideo;
    public VideoPlayer videoPlayer;
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
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;

        StorageReference videoRef = storage.GetReferenceFromUrl(caminhoNoBucket);

        // Baixe o arquivo de v�deo
        videoRef.GetBytesAsync(long.MaxValue).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Falha ao carregar o v�deo: " + task.Exception);
            }
            else
            {
                byte[] videoBytes = task.Result;
                // Use o v�deoBytes como quiser (ex: crie uma textura de v�deo ou salve o arquivo localmente)
                Debug.Log("V�deo carregado com sucesso!");
            }
        });
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
    public void enviarProposta()
    {
        descricao = textoDescricao.text;
        nomeVideo = textoNomeVideo.text;
    }
    public void EscolherVideo()
    {
        path = EditorUtility.OpenFilePanel("Mostrando todos os v�deos", "", "mp4");

        if (path != null)
        {
            print("O caminho foi salvo");
        }
    }
    public void UploadVideo()
    {
        if (path != null && textoNomeVideo.text != "")
        {
            print("Caminho para o v�deo definido com sucesso!");
        }
        else
        {
            print("Nenhum v�deo encontrado, por favor selecione um video de seu computador!");
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
            print("Por favor selecione o v�deo e preencha o nome");
        }
       
    }
    public void LoadVideoEnter()
    {
        LoadVideoFromFirebaseStorage(caminhoNoBucket + textoNomeVideo.text);
    }
}
  
