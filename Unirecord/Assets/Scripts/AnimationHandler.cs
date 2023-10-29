
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.Video;

public class AnimationHandler : MonoBehaviour
{
    [Header("Presets")]
    public Animator animator;

    public AnimationClip animclip;

    public VideoClip videoclip;

    [Header("Handler variables & Monitoring")]

    public float currentPos; //{get; private set;}

    public bool Play = false;


    void Start()
    {
        animator.Play(animclip.name, 0, 1);
    }

    public void Init()
    {
        float pos = currentPos;

        animator.Play(animclip.name, 0, 1);
        var manager = AnimationManager.Instance;
        manager.AnimUI.MediaPlayer.clip = videoclip;
        manager.AnimUI.MediaPlayer.Play();
        manager.AnimUI.MediaPlayer.Pause();
        manager.AnimUI.MediaPlayer.time = pos * videoclip.length;

        manager.AnimUI.SliderFill.color = Color.red;
        manager.AnimUI.Slider2Fill.color = Color.red;
        manager.AnimUI.SliderTransFill.color = Color.red;

        if (manager.transition == false)
        {

            if (manager.Handlers[1] == this)
            {
                manager.AnimUI.SliderFill.color = Color.yellow;
                manager.AnimUI.Slider2Fill.color = Color.red;
                manager.AnimUI.SliderTransFill.color = Color.red;
            }
            else
            {
                manager.AnimUI.SliderFill.color = Color.red;
                manager.AnimUI.Slider2Fill.color = Color.yellow;
                manager.AnimUI.SliderTransFill.color = Color.yellow;
            }
        }
        else
        {

            if (manager.Handlers[1] == this)
            {
                manager.AnimUI.SliderFill.color = Color.gray;
                manager.AnimUI.Slider2Fill.color = Color.red;
                manager.AnimUI.SliderTransFill.color = Color.red;
            }
            else
            {
                manager.AnimUI.SliderFill.color = Color.red;
                manager.AnimUI.Slider2Fill.color = Color.gray;
                manager.AnimUI.SliderTransFill.color = Color.gray;
            }
        }



    }






    void Update()
    {
        if (!AnimationManager.Instance.InVideo) return;
        if (!Play) return;
        if (AnimationManager.Instance.Handlers[0] != this && AnimationManager.Instance.Handlers[1] != this) return;

        OrientVideo();
    }
    public void OrientVideo()
    {
        if (AnimationManager.Instance.Handlers[AnimationManager.Instance.SelectedIndex] == this)
        {

            VideoPlayer MediaPlayer = AnimationManager.Instance.AnimUI.MediaPlayer;
            if (currentPos >= 0.965f)
            {
                currentPos = 0f;
                MediaPlayer.time = 0;
                if (Play) MediaPlayer.Play();
            }
            else if (currentPos < 0)
            {
                currentPos = 0.95f;
                MediaPlayer.time = MediaPlayer.length * 0.95f;
                Debug.Log("Reverse");
            }
            currentPos = Mathf.Clamp((float)MediaPlayer.time / (float)MediaPlayer.length, 0, 1);
        }


        animator.SetFloat("offset", currentPos);
    }
}
