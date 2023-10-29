using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationManager
{


    public static AnimationManager Instance;

    public AnimationHandler[] Handlers = new AnimationHandler[2];

    public int SelectedIndex = 0;

    public AnimatonHandlerUI AnimUI;

    public bool transition = false;

    public bool InVideo = true;

    public static AnimationManager Init(AnimatonHandlerUI ui)
    {
        Instance = new AnimationManager
        {
            AnimUI = ui

        };

        return Instance;
    }

    public readonly float[] TransIndex0 = { 0.25f, 0.46f, 0.67f, 0.88f };
    public readonly float[] TransIndex1 = { 0f, 0.28f, 0.56f, 0.83f };


    public void ChangeHandlers(AnimationHandler firstHandler, AnimationHandler secondHandler = null, bool _transition = false)
    {
        Handlers[0] = firstHandler;
        Handlers[1] = secondHandler;
        transition = _transition;

        AnimUI.handler = Handlers[0];
        AnimUI.MediaPlayer.time = 0;
        AnimUI.MakeOrderSliders();


    }

    public void OnPlayerChangeVideo(object sender, VideoArgs e)
    {


        if (transition != false && e.tolerance == false)
        {
            float pos = e.pos;

            if (SelectedIndex == 0)
            {
                bool isAbleToSwitch = false;
                for (int i = 0; i < TransIndex0.Length; i++)
                {
                    if (Mathf.Abs(pos - TransIndex0[i]) < 0.03f)
                    {
                        isAbleToSwitch = true;
                        break;
                    }
                }
                if (!isAbleToSwitch) return;
            }
            else
            {

                bool isAbleToSwitch = false;
                for (int i = 0; i < TransIndex1.Length; i++)
                {
                    if (Mathf.Abs(pos - TransIndex1[i]) < 0.03f)
                    {
                        isAbleToSwitch = true;
                        break;
                    }
                }
                if (!isAbleToSwitch) return;
            }

        }

        if (Handlers[0] == null)
        {
            AnimUI.CanvasGO.SetActive(false);
            return;
        }
        else AnimUI.CanvasGO.SetActive(true);

        Debug.Log("Video Changed");

        AnimUI.MediaPlayer.Pause();
        Handlers[SelectedIndex].Play = false;
        AnimUI.ButtonTMP.text = "Play";


        SelectedIndex = e.index;
        AnimUI.handler = Handlers[SelectedIndex];
        Handlers[SelectedIndex].Init();

        if (SelectedIndex == 0) AnimUI.slider.value = Handlers[SelectedIndex].currentPos;
        else
        {
            AnimUI.slider2.value = Handlers[SelectedIndex].currentPos;
            AnimUI.slidertrans.value = Handlers[SelectedIndex].currentPos;
        }

        if (transition == true)
        {

            if (SelectedIndex == 1)
            {
                AnimUI.sliderBackground.enabled = false;
                AnimUI.slider2Background.enabled = true;
                AnimUI.slidertransBackground.enabled = true;
            }
            else
            {
                AnimUI.sliderBackground.enabled = true;
                AnimUI.slider2Background.enabled = false;
                AnimUI.slidertransBackground.enabled = false;
            }
        }
        else
        {
            AnimUI.sliderBackground.enabled = true;
            AnimUI.slider2Background.enabled = true;
            AnimUI.slidertransBackground.enabled = true;
        }
    }

}


