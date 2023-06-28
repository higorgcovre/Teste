using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Vote : MonoBehaviour
{

    public TextMeshProUGUI YesVotesT, noVotesT;
    public int YesVotes, noVotes;
    public PhotonView photonView;

    public void VoteYes()
    {
        YesVotes = int.Parse(YesVotesT.text);
        YesVotes++;
        photonView.RPC("UpdateVotes", RpcTarget.All);
    }
    public void VoteNo() 
    {
        noVotes = int.Parse(noVotesT.text);
        noVotes++;
        photonView.RPC("UpdateVotes", RpcTarget.All);
    }
    [PunRPC]
    public void UpdateVotes()
    {
        YesVotesT.text = YesVotes.ToString();
        noVotesT.text = noVotes.ToString();
    }

}
