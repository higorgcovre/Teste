using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_Manager : MonoBehaviour
{
    public GameObject bancada, participant;

    public GameObject CameraLook;
    public List<GameObject> respawnsBancada, respawnsParticipante;
    public Dictionary<string, Transform> Respawns = new Dictionary<string, Transform>();

    public static bool Menu_Type;
    public void ShowCanvas()
    {
        if (participant.activeSelf || bancada.activeSelf)
            HideCanvas();
        else
        {
            if (Menu_Type)
                participant.SetActive(true);
            else bancada.SetActive(true);
        }
    }

    public void HideCanvas()
    {
        participant.SetActive(false);
        bancada.SetActive(false);
    }

}
