using TMPro;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    public TextMeshProUGUI nome;
    public bool selecionado;
    public Image[] papel;

    public static int type;

    private void Start()
    {
        nome.text = PhotonNetwork.LocalPlayer.NickName;
    }
    public void QuitLobby()
    {
        LoadSceneManager.instance.Load(0);
    }

    public void Bancada(int var)
    {
        if (var == 0)
            papel[1].color = Color.white;
        else
            papel[0].color = Color.white;

        papel[var].color = Color.blue;
        type = var;
        selecionado = true;
    }
    public void SceneEnter()
    {
        if (selecionado)
        {
            NetworkManager.Instance.AddSelecao(type, PhotonNetwork.LocalPlayer.NickName);
            LoadSceneManager.instance.Load(2);
            NetworkManager.Instance.EnterRoom();
        }
        else nome.text = "Por favor, selecione seu local no Senado.";
    }
}
