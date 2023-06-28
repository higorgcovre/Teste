using TMPro;
using UnityEngine;

public class Mute : MonoBehaviour
{
    public void MutePlayer()
    {
        FindObjectOfType<SystemUser>().MutePLayer(transform.Find("Name").transform.GetComponent<TextMeshProUGUI>().text);
    }
    public void MuteAll()
    {
        FindObjectOfType<SystemUser>().MuteAll();
    }
}
