using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Menu_Bancada : MonoBehaviour
{
    public GameObject[] Pages, Pages_Request, Pages_Presentation, Presentation_Lines, Menu;
    //Request, Vote, Presentation;
    //public GameObject R_Contact, R_Request;
    public GameObject Menu_Presentation, Notif;
    public GameObject Prefab_PartipantVoice;

    private void Start()
    {
        ChangePage(0);
    }

    public void ParticipantList()
    {

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
