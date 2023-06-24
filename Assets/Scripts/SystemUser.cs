using UnityEngine;
using Photon.Pun;
using TMPro;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;

    public Transform[] spawnsNormal;
    public Transform[] spawnsMasters;

    public TextMeshPro nome;
    public PhotonView photonView;
    void Start()
    {
        nome.text = PhotonNetwork.LocalPlayer.NickName;
        NetworkManager.Instance.spawnsMasters = spawnsMasters;
        NetworkManager.Instance.spawnsNormal = spawnsNormal;
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
