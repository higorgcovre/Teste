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
        photonView.RPC("UpdateVotes", RpcTarget.All);
    }
    public void VoteNo() 
    {
        noVote = true;
        photonView.RPC("UpdateVotes", RpcTarget.All);
    }
    [PunRPC]
    public void UpdateVotes(int _votes)
    {
        if (yesVote)
        {
            var nVotoY = int.Parse(YesVotesT.text) + 1;
            YesVotesT.text = nVotoY.ToString();
        }
        if (noVote)
        {
            var nVotoN = int.Parse(YesVotesT.text) + 1;
            noVotesT.text = nVotoN.ToString();
        }
    }

}
