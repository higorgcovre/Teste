using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ShowInfos : MonoBehaviour
{
    PhotonView view;
    public void Start()
    {
        view = GetComponent<PhotonView>();
    }
    public void UpdateVotacao(string proposta, string descricao)
    {
        view.RPC("EnviarVotacao", RpcTarget.All, proposta, descricao);
    }

    public void SendVideo(string url)
    {
        view.RPC("ShowVideo", RpcTarget.All, url);
    }

    [PunRPC]
    public void Votacao()
    {

    }

    [PunRPC]
    public void EnviarVotacao(string proposta, string descricao)
    {
        Menu_Bancada.instance.GetPropostaVote(proposta, descricao);
    }

    public void EnviarApresentacao(Proposta proposta)
    {
        view.RPC("SendPresention", RpcTarget.All, proposta.name, proposta.descricao, proposta.url);
    }

    public void ApresentarVideo()
    {

    }

    [PunRPC]
    public void ShowVideo(string url)
    {
        print("Mostrando Video para geral");
        FindObjectOfType<PlayerVideo>().IniciarVideo(url);
        FindObjectOfType<Menu_Bancada>().ShowCameraTela();
    }

    [PunRPC]

    public void SendPresention(string name, string descricao, string url)
    {
        print("Proposta Enviada");
        print(FindObjectOfType<Menu_Bancada>().name);
        FindObjectOfType<Menu_Bancada>().GetPresention(name, descricao, url);
    }



}
