using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    public Recorder recorder;
    public TextMeshPro nome;
    public PhotonView photonView;
    public bool bancada;

    public static bool change;
    void Start()
    {
        print("aaaaa");
        if (photonView.IsMine)
        {
            nome.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else ChangeName();
        recorder.TransmitEnabled = true;

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            print(PhotonNetwork.LocalPlayer.NickName);
            Menu_Manager.Menu_Type = NetworkManager.Instance.SeachSelecao(PhotonNetwork.LocalPlayer.NickName);
            if (Menu_Manager.Menu_Type)
            {
                transform.SetParent(FindObjectOfType<Menu_Manager>().respawnsParticipante[PhotonNetwork.CountOfPlayers -1].transform);
                FindObjectOfType<Menu_Manager>().Respawns.Add(PhotonNetwork.LocalPlayer.NickName, transform);
                transform.position = FindObjectOfType<Menu_Manager>().respawnsParticipante[PhotonNetwork.CountOfPlayers - 1].transform.position;
                print(transform.position);
            }
            else
            {
                transform.SetParent(FindObjectOfType<Menu_Manager>().respawnsBancada[PhotonNetwork.CountOfPlayers - 1].transform);
                FindObjectOfType<Menu_Manager>().Respawns.Add(PhotonNetwork.LocalPlayer.NickName, transform);
                transform.position = FindObjectOfType<Menu_Manager>().respawnsBancada[PhotonNetwork.CountOfPlayers - 1].transform.position;
                print(transform.position);
            }
        }
        

    }

    public void Update()
    {
    }
    public void ChangeName()
    {
        print(photonView.Owner.NickName);
        nome.text = photonView.Owner.NickName;
    }

    public void MutePLayer(string _name)
    {
        photonView.RPC("MuteRPCPlayer", RpcTarget.All, _name);
    }
    public void MuteAll()
    {
        
       photonView.RPC("MuteRPC", RpcTarget.All);
        
    }

    [PunRPC]
    public void MuteRPC()
    {
        if (!bancada)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
    }
    [PunRPC]
    public void MuteRPCPlayer(string _name)
    {
        if ( PhotonNetwork.LocalPlayer.NickName == _name)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
    }
}
