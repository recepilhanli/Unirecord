
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Video;
using System.Collections.Generic;
using Unity.Mathematics;
public class AnimatonHandlerUI : MonoBehaviour
{

    public event EventHandler<VideoArgs> OnVideoChanged;

    [Header("Manager Presets")]
    public AnimationHandler handler;
    public VideoPlayer MediaPlayer;

    [Space, Header("Slider Presets")]

    //UI

    public Slider slider;

    public Slider sliderBackground;

    public Slider slider2;

    public Slider slider2Background;

    public Slider slidertrans;

    public Slider slidertransBackground;

    public Image SliderFill;
    public Image Slider2Fill;
    public Image SliderTransFill;


    public TextMeshProUGUI ButtonTMP;
    public GameObject CanvasGO;


    [SerializeField] GameObject Slider2GameObject;
    [SerializeField] GameObject SliderTransGameObject;


    public void MakeOrderSliders()
    {
        var manager = AnimationManager.Instance;

        if (manager.Handlers[1] == null)
        {
            Slider2GameObject.SetActive(false);
            SliderTransGameObject.SetActive(false);
            return;
        }

        SliderTransGameObject.SetActive(false);
        Slider2GameObject.SetActive(false);

        if (manager.transition != false) SliderTransGameObject.SetActive(true);
        else Slider2GameObject.SetActive(true);




    }




    void Start()
    {
        var manager = AnimationManager.Init(this);
        OnVideoChanged += manager.OnPlayerChangeVideo;

        Debug.Log($"Startup handler: {handler}");
        manager.ChangeHandlers(handler, null, false);
        MediaPlayer.Pause();
        OnVideoChanged?.Invoke(this, new VideoArgs { index = 0, tolerance = true });
    }


    public void OnSliderValueChanged(int id)
    {
        if (!AnimationManager.Instance.InVideo) return;
        if (handler == null) return;

        Slider selected_Slider = null;
        Slider selectedsliderBackground = null;
        if (id == 0)
        {
            selected_Slider = slider;
            selectedsliderBackground = sliderBackground;
        }
        else
        {
            if (AnimationManager.Instance.transition != false)
            {
                selected_Slider = slidertrans;
                selectedsliderBackground = slidertransBackground;
            }
            else
            {
                selected_Slider = slider2;
                selectedsliderBackground = slider2Background;
            }

        }

        if (!selectedsliderBackground.gameObject.activeInHierarchy) return;

        if (id != AnimationManager.Instance.SelectedIndex)
        {
            float value = selectedsliderBackground.value;
            selected_Slider.value = value;
            AnimationManager.Instance.Handlers[id].currentPos = value;
            AnimationManager.Instance.Handlers[id].OrientVideo();
            Debug.Log("Slider value changed (which didnt select by player)");
        }
        else
        {
            float value = selectedsliderBackground.value;

            selected_Slider.value = value;

            float vlenght = (float)MediaPlayer.length;

            float unnormalizedValue = (value * vlenght);

            MediaPlayer.time = unnormalizedValue;
            var manager = AnimationManager.Instance;
            manager.Handlers[manager.SelectedIndex].currentPos = selected_Slider.value;

            handler.OrientVideo();
            Debug.Log("Slider value changed");
        }
    }


    void Update()
    {
        if (!AnimationManager.Instance.InVideo) return;
        if (handler == null) return;

        ArrowKeys();

        OrientSlider();

    }

    public void OrientSlider(bool ignorePlaying = false)
    {

        if (handler.Play || ignorePlaying == true)
        {

            Slider selected_Slider = null;
            if (AnimationManager.Instance.SelectedIndex == 0) selected_Slider = slider;
            else
            {
                if (AnimationManager.Instance.transition != false) selected_Slider = slidertrans;
                else selected_Slider = slider2;
            }

            selected_Slider.value = handler.currentPos;
            return;
        }

    }

    void ArrowKeys()
    {
        if (handler == null) return;
        KeyCode up = KeyCode.UpArrow;
        KeyCode down = KeyCode.DownArrow;
        KeyCode right = KeyCode.RightArrow;
        KeyCode left = KeyCode.LeftArrow;

        if (AnimationManager.Instance.Handlers[1] != null)
        {
            if (Input.GetKeyDown(up) || Input.GetKeyDown(down))
            {
                int selected = AnimationManager.Instance.SelectedIndex;
                if (selected == 1) selected = 0;
                else selected = 1;

                OnVideoChanged?.Invoke(this, new VideoArgs { index = selected, pos = handler.currentPos });
            }
        }

        if (Input.GetKey(right))
        {
            MediaPlayer.time += 0.05f;
            handler.OrientVideo();
            OrientSlider(true);
        }
        else if (Input.GetKey(left))
        {
            if (MediaPlayer.time - 0.1f > 0) MediaPlayer.time -= 0.1f;
            else handler.currentPos = -0.1f;
            handler.OrientVideo();
            OrientSlider(true);
        }

    }


    public void TogglePlay()
    {

        handler.Play = !handler.Play;

        ButtonTMP.text = (handler.Play) ? "Stop" : "Play";

        if (handler.Play) MediaPlayer.Play();
        else MediaPlayer.Pause();

        Debug.Log("Button pressed.");

    }

}

public class VideoArgs : EventArgs
{
    public int index;
    public float pos = 0f;

    public bool tolerance = false;
}


