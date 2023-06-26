using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Voice.Unity;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    public Recorder recorder;
    public TextMeshPro nome;
    public PhotonView photonView;
    void Start()
    {
        if (photonView.IsMine)
        {
            nome.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            nome.text = NetworkManager.Instance.nomeOutroPlayer;
        }

        recorder.TransmitEnabled = true;
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            float rotation = Input.GetAxis("Horizontal");
            float speed = Input.GetAxis("Vertical");

            Quaternion rot = rb.rotation * Quaternion.Euler(0, rotation * Time.deltaTime * 60, 0);
            rb.MoveRotation(rot);
            Vector3 force = rot * Vector3.forward * speed * 1000 * Time.deltaTime;

            rb.AddForce(force);

            if (rb.velocity.magnitude > 2)
            {
                rb.velocity = rb.velocity.normalized * 2;
            }
        }
    }
}
