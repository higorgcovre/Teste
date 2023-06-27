using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void EnviarApresentacao()
    {

    }

    public void ApresentarVideo()
    {

    }

    [PunRPC]
    public void ShowVideo(string url)
    {
        print("Mostrando Video para geral");
        FindObjectOfType<PlayerVideo>().IniciarVideo(url);
    }



}
