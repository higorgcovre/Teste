using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
[SerializeField]
[System.Serializable]
public class Proposta
{
    public string name, descricao, url;
}
public class Menu_Bancada : MonoBehaviour
{
    public static Menu_Bancada instance;
    public GameObject[] Pages, Pages_Request, Pages_Presentation, Presentation_Lines, Menu;
    //Request, Vote, Presentation;
    //public GameObject R_Contact, R_Request;
    public GameObject Menu_Presentation, Notif;
    public GameObject Camera;

    [Header("Participantes")]
    public GameObject contentParticipantVoice;
    public GameObject Prefab_PartipantVoice;
    public List<GameObject> ParticipantsVoice;

    [Header("Video")]
    public GameObject Prefab_VideoRecents;
    public GameObject content_VideosRecents;
    public List<GameObject> videosRecentsPrefab;
    public List<VideoRecent> videosRecents = new List<VideoRecent>();

    public VideoPlayer presention;

    [Header("Vote")]
    public TextMeshProUGUI VoteTitle;
    public TextMeshProUGUI VoteDesc, VoteTelao;
    public TextMeshProUGUI voteT, voteD;

    [Header("Presention")]
    public List<Proposta> propostas = new List<Proposta>();
    public List<GameObject> propostasSend;
    public TMP_InputField description;
    public bool participant;
    public string url;
    public int atual;
    public TextMeshProUGUI messageNum, nameParticipant, descriptionParticipant;
    public VideoPlayer videoplayer;
    public GameObject prefab_Proposta, content_proposta;

    private void Start()
    {
        instance = GetComponent<Menu_Bancada>();
        ChangePage(0);
        //ParticipantList();
        UpdatePresention();
    }

    public void AddVideoRecents(string name, string caminho)
    {
        print("a");
        VideoRecent video = new VideoRecent();
        video.name = name;
        video.caminho = caminho;
        videosRecents.Add(video);
        print("Vídeo Adicionado a Lista");
        VideoRecents();
    }


    void ClearLists(List<GameObject> list)
    {
        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
                Destroy(list[i]);
            list.Clear();
        }
    }

    public void ParticipantList()
    {
        ClearLists(ParticipantsVoice);
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            InstantiatePrefabParticipantVoice(i);
    }

    public void VideoRecents()
    {
        print("Atualizar Lista de Vídeos");
        ClearLists(videosRecentsPrefab);
        for (int i = 0; i < videosRecents.Count; i++)
            InstantiatePrefabVideoRecents(i);
    }

    void InstantiatePrefabParticipantVoice(int i)
    {
        GameObject obj = Instantiate(Prefab_PartipantVoice);
        obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[i].NickName;
        obj.transform.SetParent(contentParticipantVoice.transform, false);
        ParticipantsVoice.Add(obj);
    }

    void InstantiatePrefabVideoRecents(int i)
    {
        GameObject obj = Instantiate(Prefab_VideoRecents);
        obj.transform.Find("fileName").GetComponent<TextMeshProUGUI>().text = videosRecents[i].name;
        obj.name = videosRecents[i].name;
        obj.transform.SetParent(content_VideosRecents.transform, false);
        videosRecentsPrefab.Add(obj);
    }

    public void ChangePage(int page)
    {
        EnablePages(page, Pages);
        for (int i = 0; i < Menu.Length; i++)
        {
            if (page == i)
                Menu[i].GetComponent<CanvasGroup>().alpha = 1;
            else Menu[i].GetComponent<CanvasGroup>().alpha = 0.2f;
        }
    }
    void EnablePages(int page, GameObject[] Pages)
    {
        for (int i = 0; i < Pages.Length; i++)
        {
            if (page == i)
                Pages[i].SetActive(true);
            else Pages[i].SetActive(false);
        }
    }

    public void ChangeRequest(int page)
    {
        EnablePages(page, Pages_Request);
    }

    public void ChangeRecents(int page)
    {
        EnablePages(page, Pages_Presentation);
        
        EnablePages(page, Presentation_Lines);
        Menu_Presentation.SetActive(true);
    }

    public void OpenVideo(string videoUrl)
    {
        Menu_Presentation.SetActive(false);
        EnablePages(2, Pages_Presentation);

        print("Enviando Vídeo");
        if (!participant)
            FindObjectOfType<ShowInfos>().SendVideo(videoUrl);
        else url = videoUrl;
        //FindObjectOfType<PlayerVideo>().IniciarVideo(videoUrl);
    }

    public void ShowCameraTela()
    {
        Transform posRespaw = FindObjectOfType<Menu_Manager>().Respawns[PhotonNetwork.LocalPlayer.NickName].Find("Camera");
        Camera = FindObjectOfType<Menu_Manager>().CameraLook;
        Camera.transform.position = posRespaw.position;
        Camera.SetActive(true);
        //FindObjectOfType<Menu_Manager>().Respawns[PhotonNetwork.LocalPlayer.NickName].Find("Camera").gameObject.SetActive(true);
        //presention.targetCamera = FindObjectOfType<Menu_Manager>().Respawns[PhotonNetwork.LocalPlayer.NickName].Find("Camera").GetComponent<Camera>().
    }

    public void OpenRequest()
    {
        EnablePages(1, Pages_Request);
    }

    public void SetPropostaVote()
    {
        FindObjectOfType<ShowInfos>().UpdateVotacao(voteT.text, voteD.text);
        print("Proposta Enviar");
    }

    public void OpenPresention(int i)
    {
        atual = i;
        nameParticipant.text = propostas[i].name;
        descriptionParticipant.text = propostas[i].descricao;
        descriptionParticipant.text = propostas[i].descricao;
        videoplayer.url = propostas[i].url;
    }
    public void AceitarPresention()
    {
        StartPresention();
    }

    public void RecusePresention()
    {
        Destroy(propostasSend[atual]);
        propostas.RemoveAt(atual);

        ChangeRequest(0);
    }

    public void Mutar()
    {
        FindObjectOfType<SystemUser>().MuteRPC();
    }
    public void GetPropostaVote(string name, string desc)
    {
        VoteTitle.text = name;
        VoteDesc.text = desc;
        VoteTelao.text = desc;
    }

    public void SetPresention()
    {
        Proposta proposta = new Proposta();
        proposta.name = PhotonNetwork.LocalPlayer.NickName;
        proposta.descricao = description.text;
        proposta.url = url;
        FindObjectOfType<ShowInfos>().EnviarApresentacao(proposta);
    }

    public void GetPresention(string name, string descricao, string url)
    {
        print("Atualizando Propostas");
        Proposta proposta = new Proposta();
        proposta.name = name;
        proposta.descricao = descricao;
        proposta.url = url;

        propostas.Add(proposta);
        UpdatePresention();
    }

    void UpdatePresention()
    {
        print("Mostrando Propostas");

        ClearLists(propostasSend);

        for (int i = 0; i < propostas.Count; i++)
        {
            InstantiatePrefabProposta(i, propostas[i]);
        }
        messageNum.text = propostas.Count.ToString();
    }
    void InstantiatePrefabProposta(int i, Proposta proposta)
    {
        GameObject obj = Instantiate(prefab_Proposta);
        obj.transform.Find("name").GetComponent<TextMeshProUGUI>().text = proposta.name;
        obj.name = i.ToString();
        obj.transform.SetParent(content_proposta.transform, false);
        propostasSend.Add(obj);
        
    }

    public void StartPresention()
    {
        presention.url = PlayerVideo.urlVideo;
        presention.Play();
        print("Apresentar Vídeo");
    }

    public void CloseAba()
    {
        if (Pages_Presentation[2].activeSelf)
            ChangeRecents(0);
        else ChangeRequest(0);
    }
}
