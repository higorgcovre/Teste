using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Menu_Bancada : MonoBehaviour
{
    public GameObject[] Pages, Pages_Request, Pages_Presentation, Presentation_Lines, Menu;
    //Request, Vote, Presentation;
    //public GameObject R_Contact, R_Request;
    public GameObject Menu_Presentation, Notif;
    public GameObject contentParticipantVoice;
    public GameObject Prefab_PartipantVoice;

    public List<GameObject> ParticipantsVoice;


    private void Start()
    {
        ChangePage(0);
        ParticipantList();
    }

    public void ParticipantList()
    {
        if (ParticipantsVoice.Count > 0)
        {
            for (int i = 0; i < ParticipantsVoice.Count; i++)
                Destroy(ParticipantsVoice[i]);
            ParticipantsVoice.Clear();
        }
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                InstantiatePrefabParticipantVoice(i);
    }

    void InstantiatePrefabParticipantVoice(int i)
    {
        GameObject obj = Instantiate(Prefab_PartipantVoice);
        obj.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = PhotonNetwork.PlayerList[i].NickName;
        obj.transform.SetParent(contentParticipantVoice.transform, false);
        ParticipantsVoice.Add(obj);
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

    public void OpenVideo()
    {
        Menu_Presentation.SetActive(false);
        EnablePages(2, Pages_Presentation);
    }

    public void OpenRequest()
    {
        EnablePages(1, Pages_Request);
    }


    public void CloseAba()
    {
        if (Pages_Presentation[2].activeSelf)
            ChangeRecents(0);
        else ChangeRequest(0);
    }
}
