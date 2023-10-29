
using Cinemachine;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{

    /// <summary>
    /// Static instance of player.
    /// </summary>
    public static Player Instance { get; private set; }

    [Header("Setting Up")]
    public CharacterController controller;
    public Camera PlayerCamera;
    public Camera ScreenCamera;
    public CinemachineVirtualCamera vCam;
    private Cinemachine.CinemachineBasicMultiChannelPerlin perlin;
    [SerializeField] Volume GlobalVolume;
    private ColorAdjustments adjustments;

    [Header("Player Presets")]

    [SerializeField] float Speed = 10f;

    public bool Busy = true;


    void Start()
    {
        Instance = this;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;

        perlin = vCam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        GlobalVolume.profile.TryGet(out adjustments);

        adjustments.postExposure.value = Menu.Brightness * 4;

    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("Menu");

        if (!ScreenCamera.gameObject.activeInHierarchy)
        {
            Vector3 euler = transform.eulerAngles;
            euler.y = PlayerCamera.transform.eulerAngles.y;
            transform.eulerAngles = euler;
        }


        if (Busy) return;

        if (Input.GetMouseButtonDown(1))
        {
            ScreenCamera.gameObject.SetActive(PlayerCamera.gameObject.activeInHierarchy);
            PlayerCamera.gameObject.SetActive(!PlayerCamera.gameObject.activeInHierarchy);

            Cursor.visible = !PlayerCamera.gameObject.activeInHierarchy;
            Cursor.lockState = PlayerCamera.gameObject.activeInHierarchy ? CursorLockMode.Locked : CursorLockMode.None;

        }


        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 forward = transform.forward * v * Speed * Time.deltaTime;

        Vector3 right = transform.right * h * Speed * Time.deltaTime;

        if (forward != Vector3.zero || right != Vector3.zero) controller.Move(forward + right + new Vector3(0, -1, 0));

        if (forward != Vector3.zero || right != Vector3.zero)
        {
            Shake(0.9f, 0.022f);
        }
        else Shake(0.2f, 0.008f);


    }


    void Shake(float Amplitude, float Frequency)
    {
        perlin.m_AmplitudeGain = Amplitude;
        perlin.m_FrequencyGain = Frequency;
    }

}
