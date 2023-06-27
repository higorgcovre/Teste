using Photon.Pun;
using Photon.Voice.Unity;
using UnityEngine;

public class VoiceChatManager : MonoBehaviourPunCallbacks
{
    private Recorder recorder;

    public Sprite enable, disable;

    private void Start()
    {
        recorder = FindObjectOfType<Recorder>();

        // Conectar-se ao Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // Entrar em uma sala específica
        PhotonNetwork.JoinOrCreateRoom("Senado", new Photon.Realtime.RoomOptions(), null);
    }

    public void Mute()
    {
        //if (!photonView.IsMine) 
        //{
            recorder.TransmitEnabled = !recorder.TransmitEnabled;
        //}

        if (recorder.TransmitEnabled)
            GetComponent<UnityEngine.UI.Image>().sprite = enable;
        else GetComponent<UnityEngine.UI.Image>().sprite = disable;
    }
}
public class Avatar : MonoBehaviourPun
{
    public TextMesh playerNameText;

    private void Start()
    {
        // Atribuir o nome de identificação do jogador ao avatar
        playerNameText.text = photonView.Owner.NickName;
    }
}
public class RoomManager : MonoBehaviourPunCallbacks
{
    public override void OnJoinedRoom()
    {
        // Instanciar o avatar do jogador na sala
        PhotonNetwork.Instantiate("AvatarPrefab", Vector3.zero, Quaternion.identity, 0);
    }
}
public class VoiceChat : MonoBehaviourPunCallbacks
{
    private Recorder recorder;

    public void Mute()
    {
        // Obter o componente Recorder
        recorder = GetComponent<Recorder>();

        // Verificar se o jogador local é o dono do avatar
        if (photonView.IsMine)
        {
            // Ativar o microfone do jogador local
            recorder.TransmitEnabled = true;
        }
        else
        {
            // Desativar a reprodução do áudio para os outros jogadores
            recorder.RecordingEnabled = false;
        }
    }
}