using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Photon.Voice.Unity;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }
    
    public Transform[] spawnsNormal;
    public Transform[] spawnsMasters;
    public string nomeOutroPlayer;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            gameObject.SetActive(false);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    //--------------------------------------------------------------------------
    public override void OnConnectedToMaster()
    {
        print("Conexão Phonton Bem Sucedida");
    }
    //---------------------------------------------------------------------------
    public void EnterLobby(string playerName)
    {
        PhotonNetwork.LocalPlayer.NickName = playerName;
        
        if(!PhotonNetwork.InLobby)
        {
            print("Entrando no Lobby...");
            PhotonNetwork.JoinLobby();
        }
    }
    //--------------------------------------------------------------------------
    public override void OnJoinedLobby()
    {
        print("Entrei no Lobby com o Nome: "+ PhotonNetwork.LocalPlayer.NickName);
    }
    //---------------------------------------------------------------------------
    public override void OnLeftRoom()
    {
        print("Você saiu da sala Auditório");
    }
    //---------------------------------------------------------------------------
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Erro ao entrar na sala: " + message + "Código: " + returnCode);
        
        if(returnCode == ErrorCode.GameDoesNotExist)
        {
            print("Criando a sala Auditório");
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 20 };
            PhotonNetwork.CreateRoom("Auditorio", roomOptions, null);
        }
    }
    //---------------------------------------------------------------------------
    public void EnterRoom()
    {
        print("Entrando sala auditorio...");
        PhotonNetwork.JoinRoom("Auditorio");
    }

    public override void OnJoinedRoom()
    {
        print("Você entrou na sala!");

        PhotonNetwork.Instantiate("(RIG) BodyMan_1_SystemUser Variant", spawnsNormal[0].position, Quaternion.identity);
    }
    //---------------------------------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Um jogador está entrou na sala, o Nome dele é: " + newPlayer.NickName);
        nomeOutroPlayer = newPlayer.NickName;
    }
    //---------------------------------------------------------------------------
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("Um jogador está saiu da sala, o Nome dele era: " + otherPlayer.NickName);
    }
    //---------------------------------------------------------------------------
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
       print("Aconteceu um erro: " + errorInfo.Info);
    }
}
