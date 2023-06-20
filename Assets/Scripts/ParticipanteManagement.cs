using Firebase.Storage;
using Firebase;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ParticipanteManagement : MonoBehaviour
{
    private int nVotosSim, nVotosNao;
    [SerializeField] private TextMeshProUGUI nVotosSimT, nVotosNaoT;
    [SerializeField] private bool permitidoVotar = true;
    [SerializeField] private string descricao, nomeVideo;
    [SerializeField] private TextMeshProUGUI textoDescricao, textoNomeVideo;
    public VideoPlayer videoPlayer;
    private string path;
    private string caminhoNoBucket = "gs://teste-6010d.appspot.com/Videos";
    
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
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
    void UploadVideo(string caminhoLocalDoVideo)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference videoRef = storage.GetReferenceFromUrl(caminhoNoBucket);

        videoRef.PutFileAsync(caminhoLocalDoVideo)
            .ContinueWith((Task<StorageMetadata> task) => {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.LogError("Falha ao carregar o vídeo: " + task.Exception);
                }
                else
                {
                    Debug.Log("Vídeo carregado com sucesso!");
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
        path = EditorUtility.OpenFilePanel("Mostrando todos os vídeos", "", "mp4");
        
        if (path != null)
        {
            UploadVideo(path);
        }
    }
    public void votacaoEnter()
    {

    }
}
