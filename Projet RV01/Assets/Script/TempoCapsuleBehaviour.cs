using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoCapsuleBehaviour : MonoBehaviour
{
    Tempo tempoManager;
    // Start is called before the first frame update
    void Start()
    {
        tempoManager = Object.FindObjectOfType<Tempo>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Tempo")
        {
            tempoManager.HandleObjectCollision();
        }
    }
}
