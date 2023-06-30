using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Manager : MonoBehaviour
{
    public GameObject bancada, participant;

    public static bool Menu_Type;
    public void ShowCanvas()
    {
        if (Menu_Type)
            participant.SetActive(true);
        else bancada.SetActive(true);
    }

    public void HideCanvas()
    {
        participant.SetActive(false);
        bancada.SetActive(false);
    }

}
