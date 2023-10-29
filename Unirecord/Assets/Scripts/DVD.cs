using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DVD : MonoBehaviour
{
    [SerializeField] AnimationHandler Handler1;

    [SerializeField] AnimationHandler Handler2;

    [SerializeField] bool Transition = false;

    [SerializeField, Range(0, 1)] int Selection = 0;

    void OnTriggerEnter(Collider other)
    {

        if (!other.CompareTag("Player")) return;




        var manager = AnimationManager.Instance;
        Invoke("ActiveMe", 5f);
        gameObject.SetActive(false);
        
        if (manager.Handlers[0] == Handler1 && Handler2 == null && manager.Handlers[1] != null) return;
        else if (manager.Handlers[0] == Handler1 && Handler2 == manager.Handlers[1]) return;

        manager.ChangeHandlers(Handler1, Handler2, Transition);
        manager.OnPlayerChangeVideo(this, new VideoArgs { index = Selection, tolerance = true });


    }

    void Update()
    {
        transform.Rotate(0, 0, 100 * Time.deltaTime);
    }

    void ActiveMe()
    {
        gameObject.SetActive(true);
    }

}
