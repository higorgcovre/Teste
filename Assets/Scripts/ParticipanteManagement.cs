using TMPro;
using UnityEditor;
using UnityEngine;
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
            videoPlayer.url = path;
            videoPlayer.Play();
        }
    }
}
