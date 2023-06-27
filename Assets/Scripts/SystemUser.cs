using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    public Recorder recorder;
    public TextMeshPro nome;
    public PhotonView photonView;

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
            FindObjectOfType<Menu_Bancada>().ParticipantList();

    }

    public void ChangeName()
    {
        print(photonView.Owner.NickName);
        nome.text = photonView.Owner.NickName;
    }

    [PunRPC]
    public void MuteRPC()
    {
        recorder.TransmitEnabled = !recorder.TransmitEnabled;
    }
    [PunRPC]
    public void MuteRPCPlayer()
    {
        if ( PhotonNetwork.LocalPlayer.NickName == name)
        {
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        }
    }
}
