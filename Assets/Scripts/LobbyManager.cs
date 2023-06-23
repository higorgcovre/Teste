using TMPro;
using UnityEngine;
using Photon.Pun;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI nome;

    private void Start()
    {
        nome.text = PhotonNetwork.LocalPlayer.NickName;
    }
    public void QuitLobby()
    {
        LoadSceneManager.instance.Load(0);
    }
    public void SceneEnter()
    {
        LoadSceneManager.instance.Load(2);
    }
}
