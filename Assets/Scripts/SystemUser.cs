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
    public List<GameObject> respawnsBancada, respawnsParticipante;
    public Dictionary<string, Transform> Respawns = new Dictionary<string, Transform>();

    public static bool change;
    void Start()
    {
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
        }
        if (Menu_Manager.Menu_Type)
        {
            transform.SetParent(respawnsParticipante[0].transform);
            Respawns.Add(PhotonNetwork.LocalPlayer.NickName, transform);
        }
        else
        {
            transform.SetParent(respawnsBancada[0].transform);
            Respawns.Add(PhotonNetwork.LocalPlayer.NickName, transform);
        }

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
