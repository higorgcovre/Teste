using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfos : MonoBehaviour
{
    public void ShowMessage()
    {
        GetComponent<PhotonView>().RPC("Message", RpcTarget.All);
    }

    [PunRPC]
    public void Message()
    {
        print("opa, opa, opa");
    }

}
