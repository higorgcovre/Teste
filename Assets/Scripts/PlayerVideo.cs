using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[SerializeField]
public class VideoRecent
{
    public string name, caminho;
}
public class PlayerVideo : MonoBehaviour
{
    public static PlayerVideo instance;
    public static string urlVideo;
    //public Gerenciador_Video G_Video;
    public Slider s;
    public VideoPlayer videoPlayer, videoPresention;
    public Sprite pauseSprite, playSprite;
    public Image playButton;

    public TextMeshProUGUI t_Atual, t_total;

    public bool startVideo, button, fimVideo, pause, force;
    public float frame;
    void Start()
    {
        if(gameObject.name == "Menu")
        instance = GetComponent<PlayerVideo>();
    }
    void Update()
    {
        if (startVideo)
        {
            if (!button && !pause && !force)
            {
                atualizarSlider();
            }
            if(s.value >= 0.95f)
            {
                fimVideo = true;
                pause = true;
                playButton.sprite = playSprite;
            }

            if (s.value <= 0.015f && button)
            {
                t_Atual.text = "00:00";
            }
            else if (button)
            {
                frame = (float)s.value * (float)videoPlayer.frameCount;
                videoPlayer.frame = (long)frame;
                t_Atual.text = corrigirTempo((int)videoPlayer.time).ToString();
            }
            videoPresention = videoPlayer;
        }
    }

   
    public string corrigirTempo(int tempo)
    {
        int min = tempo % 3600 / 60;
        int seg = tempo % 60;
        return string.Format("{0:D2}:{1:D2}", min, seg);
    }

    public void IniciarVideo(string url)
    {
        videoPlayer.targetTexture.Release();
        videoPlayer.url = url;
        videoPresention.url = url;
        urlVideo = url;
        //videoPlayer.Play();
        //videoPlayer.clip = videoClip;
        double time = videoPlayer.frameCount / videoPlayer.frameRate;
        t_Atual.text = "00:00";
        t_total.text = corrigirTempo((int)time);
        s.value = 0;
        pause = true;
        playButton.sprite = pauseSprite;
        videoPlayer.Play();
        videoPresention.Play();
    }

    void atualizarSlider()
    {
        Canvas.ForceUpdateCanvases();
        s.value = (float)videoPlayer.frame / (float)videoPlayer.frameCount;
        //double time = videoPlayer.frameCount / videoPlayer.frameRate;
        t_Atual.text = corrigirTempo((int)videoPlayer.time).ToString();
        Canvas.ForceUpdateCanvases();
    }

    public void PressionarButton()
    {
        if (startVideo)
        {
            button = true;
            pause = true;
            playButton.sprite = playSprite;
            videoPlayer.Pause();
        }
    }
    public void SoltarButton()
    {
        if (startVideo)
        {
            button = false;
            if (s.value <= 0.015f)
            {
                button = false;
                pause = false;

                videoPlayer.Stop();

                double time = videoPlayer.frameCount / videoPlayer.frameRate;
                t_Atual.text = "00:00";
                t_total.text = corrigirTempo((int)time);
                s.value = 0;

                playButton.sprite = pauseSprite;
                videoPlayer.Play();
            }
            else if (s.value >= 0.99f)
            {
                playButton.sprite = playSprite;
                fimVideo = true;
                pause = true;
            }
            else
            {
                button = false;
                pause = false;

                frame = (float)s.value * (float)videoPlayer.frameCount;
                videoPlayer.frame = (long)frame;
                t_Atual.text = corrigirTempo((int)videoPlayer.time).ToString();

                StartCoroutine("forcar");
            }
        }
        
    }

    IEnumerator forcar()
    {
        force = true;
        while (videoPlayer.frame != (long)frame)
        {
            yield return new WaitForEndOfFrame();
        }

        playButton.sprite = pauseSprite;
        videoPlayer.Play();
        force = false;
        yield return new WaitForEndOfFrame();

    }
    public void VideoControlPause()
    {
        if (!startVideo)
        {
            s.gameObject.SetActive(false);
            s.value = 0;
            s.gameObject.SetActive(true);
        }
        if (pause || s.value >= 0.95f)
        {
            pause = false;
            if (s.value >= 0.95f)
            {
                fimVideo = false;
                videoPlayer.Stop();

                double time = videoPlayer.frameCount / videoPlayer.frameRate;
                t_Atual.text = "00:00";
                t_total.text = corrigirTempo((int)time);
                s.value = 0;
            }
            playButton.sprite = pauseSprite;
            videoPlayer.Play();
            startVideo = true;
        }
        else
        {
            pause = true;
            playButton.sprite = playSprite;
            videoPlayer.Pause();

        }
    }
}
