using Firebase.Storage;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class Menu_MuteParticipant : MonoBehaviour
{
    public TMP_InputField inputField;
    public string seach = "";
    public List<GameObject> seachList;

    public bool delete;

    private void Update()
    {
        if (seach != inputField.text)
            SeachParticipants();
        if (seach == "" && delete)
            DeleteSeach();
    }

    public void SeachParticipants()
    {
        delete = true;
        seach = inputField.text;
        print(seach);

        if (seachList.Count > 0)
            seachList.Clear();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].NickName.ToUpper().Contains(seach.ToUpper()))
            {
                seachList.Add(FindObjectOfType<Menu_Bancada>().ParticipantsVoice[i]);
            }
            else Destroy(FindObjectOfType<Menu_Bancada>().ParticipantsVoice[i]);
        }

        print(seachList.Count);
    }

    public void DeleteSeach()
    {
        print("Reset");
        delete = false;
        FindObjectOfType<Menu_Bancada>().ParticipantList();
    }
}
