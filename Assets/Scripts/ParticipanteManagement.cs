using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParticipanteManagement : MonoBehaviour
{
    public int nVotosSim, nVotosNao;
    public TextMeshProUGUI nVotosSimT, nVotosNaoT, textoDescricao;
    public bool permitidoVotar = true;
    public string descricao;

    public void votarSim()
    {
        if (permitidoVotar)
        {
            nVotosSim++;
            nVotosSimT.text = nVotosSim.ToString();
        }
        permitidoVotar = !permitidoVotar;

    }
    public void votarNao()
    {
        if (permitidoVotar)
        {
            nVotosNao++;
            nVotosNaoT.text = nVotosNao.ToString();
        }
        permitidoVotar = !permitidoVotar;
    }
    public void enviarProposta()
    {
       
    }
}
