using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance { get; private set; }
    
    public Transform[] spawnsNormal;
    public Transform[] spawnsMasters;
    public string nomeOutroPlayer;

    public Dictionary<string, bool> Selecao = new Dictionary<string, bool>(); //False � Bancada; True � Participant


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
        print("Conex�o Phonton Bem Sucedida");
    }
    //---------------------------------------------------------------------------
    public void EnterLobby(string playerName)
    {
        PhotonNetwork.LocalPlayer.NickName = playerName;
        
        if(!PhotonNetwork.InLobby)
        {
            AddCharacter(playerName);
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
        print("Voc� saiu da sala Audit�rio");
    }
    //---------------------------------------------------------------------------
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print("Erro ao entrar na sala: " + message + "C�digo: " + returnCode);
        
        if(returnCode == ErrorCode.GameDoesNotExist)
        {
            print("Criando a sala Audit�rio");
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
        print("Voc� entrou na sala!");

        PhotonNetwork.Instantiate("(RIG) BodyMan_1_SystemUser Variant", new Vector3(0,0,0), Quaternion.identity);
    }
    //---------------------------------------------------------------------------
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("Um jogador est� entrou na sala, o Nome dele �: " + newPlayer.NickName);

    }
    //---------------------------------------------------------------------------
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        print("Um jogador est� saiu da sala, o Nome dele era: " + otherPlayer.NickName);
    }
    //---------------------------------------------------------------------------

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
       print("Aconteceu um erro: " + errorInfo.Info);
    }

    public void AddCharacter(string name)
    {
        Selecao.Add(name, false);
        print(Selecao.Count);
    }

    public void AddSelecao(int var, string name)
    {
        print(name);
        if (var == 1 && Selecao.ContainsKey(name))
            Selecao[name] = true;
        ShowSelecao();
    }

    public bool SeachSelecao(string name)
    {
        if (Selecao.ContainsKey(name))
            return Selecao[name];
        Debug.LogError("N�o tem esse cara aqui n�o");
        return false;

    }
    public void ShowSelecao()
    {
        foreach (var kvp in Selecao)
        {
            print(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
}
