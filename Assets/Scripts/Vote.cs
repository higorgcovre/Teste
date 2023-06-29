using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vote : MonoBehaviourPunCallbacks
{

    public TextMeshProUGUI YesVotesT, noVotesT;
    public bool yesVote, noVote;

    public void VoteYes()
    {
        yesVote = true;
        var nVotoY = int.Parse(YesVotesT.text);
        photonView.RPC("UpdateVotes", RpcTarget.All, nVotoY);
    }
    public void VoteNo() 
    {
        noVote = true;
        var nVotoN = int.Parse(YesVotesT.text);
        photonView.RPC("UpdateVotes", RpcTarget.All, nVotoN);
    }
    [PunRPC]
    public void UpdateVotes(int _nVoto)
    {
        if (yesVote)
        {
            YesVotesT.text = (_nVoto+1).ToString();
        }
        if (noVote)
        {
            noVotesT.text = (_nVoto - 1).ToString();
        }
    }

}
