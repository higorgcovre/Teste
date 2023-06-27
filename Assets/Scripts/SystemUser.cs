using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;
using Photon.Voice.PUN;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    public Recorder recorder;
    public TextMeshPro nome;
    public PhotonView photonView;

    public string playerName;
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
        nome.text = photonView.Owner.NickName;
    }
    private void MuteAllOtherPlayers()
    {
        var playerVoices = FindObjectsOfType<PhotonVoiceView>();

        foreach (var playerVoice in playerVoices)
        {
            if (!photonView.IsMine) 
            { 
                recorder.TransmitEnabled = false;
            }
        }
    }
    private void MutePlayer()
    {
        var playerVoices = FindObjectsOfType<PhotonVoiceView>();
        //playerName = 
        foreach (var playerVoice in playerVoices)
        {
            if (playerVoice.gameObject.GetComponent<PhotonView>().Owner.NickName == playerName)
            {
                var recorder = playerVoice.GetComponent<Recorder>();
                recorder.TransmitEnabled = false;
                break;
            }
        }
    }
    void Update()
    {
    }
}
