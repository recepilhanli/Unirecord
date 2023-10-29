using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFinisher : MonoBehaviour
{

[SerializeField]
bool LoadMenu = false;

void Start()
{
    if(LoadMenu) Invoke("LoadMenuScene",5f);
}

void LoadMenuScene()
{
    SceneManager.LoadScene("Menu");
}

void OnTriggerEnter(Collider other)
{
    if(!other.gameObject.CompareTag("Player")) return;

    SceneManager.LoadScene("Finish");
}
}
