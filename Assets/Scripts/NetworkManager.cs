using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }
    
    public Transform[] spawnsNormal;
    public Transform[] spawnsMasters;
    public string nomeOutroPlayer;

    public List<GameObject> players;
    public int playerOutro;

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

        GameObject obj = PhotonNetwork.Instantiate("(RIG) BodyMan_1_SystemUser Variant", spawnsNormal[0].position, Quaternion.identity);
        print(spawnsNormal[0]);
        obj.transform.SetParent(spawnsNormal[0], false);
        obj.name = PhotonNetwork.LocalPlayer.NickName;
        print(obj.name);
        
       
    }
    //---------------------------------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Um jogador está entrou na sala, o Nome dele é: " + newPlayer.NickName);
        //photonView.RPC("changeName", RpcTarget.All);
        changeName();
    }
    //---------------------------------------------------------------------------
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("Um jogador está saiu da sala, o Nome dele era: " + otherPlayer.NickName);
    }
    //---------------------------------------------------------------------------
    //[PunRPC]
    public void changeName()
    {
        print(players.Count);
        print(PhotonNetwork.PlayerList.Length);

        //for(int i = 0; i < spawnsNormal[0].childCount; i++)
            

        if (PhotonNetwork.PlayerList.Length > 1)
        {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    spawnsNormal[0].GetChild(i).GetComponent<SystemUser>().ChangeName(PhotonNetwork.PlayerList[i].NickName);
                    print(PhotonNetwork.PlayerList[i].NickName);
                }

        }
        //playerOutro = PhotonNetwork.PlayerList.Length - 1;
        //SystemUser.change = true;
        //nomeOutroPlayer = newPlayer.NickName;
    }
    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
       print("Aconteceu um erro: " + errorInfo.Info);
    }
}
