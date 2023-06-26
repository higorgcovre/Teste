using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Voice.Unity;
using UnityEngine.SceneManagement;

public class SystemUser : MonoBehaviour
{
    public Rigidbody rb;
    public Recorder recorder;
    public TextMeshPro nome;
    public PhotonView photonView;

    public static bool change;
    void Start()
    {
        if (photonView.IsMine)
        {
            nome.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else ChangeName();
        recorder.TransmitEnabled = true;

        if (SceneManager.GetActiveScene().buildIndex == 2)
            FindObjectOfType<Menu_Bancada>().ParticipantList();

    }

    public void ChangeName()
    {
        print(photonView.Owner.NickName);
        nome.text = photonView.Owner.NickName;
    }

    void Update()
    {
        //if (change)
        //{
        //    change = false;
        //    ChangeName();
        //}

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
