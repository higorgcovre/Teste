using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class Menu_Bancada : MonoBehaviour
{
    public static Menu_Bancada instance;
    public GameObject[] Pages, Pages_Request, Pages_Presentation, Presentation_Lines, Menu;
    //Request, Vote, Presentation;
    //public GameObject R_Contact, R_Request;
    public GameObject Menu_Presentation, Notif;

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
    public TextMeshProUGUI VoteDesc;
    public TextMeshProUGUI voteT, voteD;


    private void Start()
    {
        instance = GetComponent<Menu_Bancada>();
        ChangePage(0);
        ParticipantList();
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

        FindObjectOfType<ShowInfos>().SendVideo(videoUrl);
        //FindObjectOfType<PlayerVideo>().IniciarVideo(videoUrl);
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

    public void Mutar()
    {
        FindObjectOfType<SystemUser>().MuteRPC();
    }
    public void GetPropostaVote(string name, string desc)
    {
        VoteTitle.text = name;
        VoteDesc.text = desc;
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
