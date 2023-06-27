using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Mute : MonoBehaviour
{
    public void MutePlayer()
    {
        FindObjectOfType<SystemUser>().MuteRPCPlayer(transform.Find("Name").transform.GetComponent<TextMeshProUGUI>().text);
    }
}
