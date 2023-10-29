using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static float Brightness = -0.1f;

    [SerializeField] Slider BrightnessSlider;

    [SerializeField] Volume GlobalVolume;

    ColorAdjustments adjustments;

    void Start()
    {
        GlobalVolume.profile.TryGet(out adjustments);

        BrightnessSlider.value = Brightness;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void OnSliderChanged()
    {
        Brightness = BrightnessSlider.value;
        adjustments.postExposure.value = Brightness * 4;
    }

    public void Play()
    {
        SceneManager.LoadScene("FirstLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
