using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;



// TESTER AVEC CASQUE 
public class Intensite : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action of Trigger (gachette clic left).")]
    InputActionReference Trigger;
    bool isActive = false;

    public Transform controller; // contrôleur
    public Transform headset; // casque
    RayCasting rc;
    public float minHeight; // hauteur minimale
    public float maxHeight;  // hauteur maximale

    private float initialHeight; // hauteur de départ du contrôleur
    private float intensite = 1;
    private float dIntensite = 0;

    private float originPositionController;

    private GameObject[] audioSources;

    void Start()
    {
        rc = Object.FindObjectOfType<RayCasting>();
        if (controller != null)
        {
            initialHeight = controller.position.y; //  hauteur initiale du contrôleur
        }
        audioSources = new GameObject[21];
        GameObject[] go = Object.FindObjectsOfType<GameObject>();
        int i = 0;
        foreach (GameObject obj in go)
        {
            if(obj.layer == 8)
            {
                audioSources[i++] = obj;
                Debug.Log("Added Audio Source : " + obj.tag);
            }
        }



    }

    void Update()
    {
        GameObject reference = rc.getSelectedObject();
        if (controller != null  && isActive)
        {
            // retourner une valeur entre 0 et 2  donc changer ca 
            // calcul différence de hauteur depuis la position initiale
            float deltaHeight = Mathf.Clamp(controller.position.y - originPositionController, minHeight, maxHeight);

            // normalisation de la différence pour qu'elle soit dans l'intervalle [-1, 1]
            dIntensite = ((deltaHeight - minHeight) / (maxHeight - minHeight)) * 2.0f - 1.0f;
            Debug.Log("deltaHeight : " + deltaHeight);

            Debug.Log("dIntensite : " + dIntensite);

            foreach (GameObject obj in audioSources)
            {
                if (reference.CompareTag(obj.tag))
                {
                    obj.GetComponent<StudioEventEmitter>().setIntensity(Mathf.Clamp(intensite + dIntensite, 0, 2));
                }
            }

        } else if (controller != null)
        {
            if (controller.position.y > headset.position.y + 0.2)
            {
                foreach (GameObject obj in audioSources)
                {
                    if (!reference.CompareTag(obj.tag))
                    {
                        obj.GetComponent<StudioEventEmitter>().setIntensity(0);
                    }
                }
            }
        }
    }
    private void OnEnable()
    {
        Trigger.action.Enable(); // activer action
        Trigger.action.performed += OnTrigger; // listener pour le trigger

    }

    private void OnDisable()
    {
        Trigger.action.performed -= OnTrigger; // retirer listener pour le trigger
        Trigger.action.Disable(); // deactiver action

    }

    private void OnTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("OnTrigger");
        originPositionController = controller.position.y;
        isActive = !isActive;
        GameObject reference = rc.getSelectedObject();
        if (isActive)
        {
            foreach (GameObject obj in audioSources)
            {
                if (obj.CompareTag(reference.tag))
                {
                    intensite = obj.GetComponent<StudioEventEmitter>().getIntensity();
                    break;
                }
            }
        }
        else
        {
            foreach (GameObject obj in audioSources)
            {
                if (obj.CompareTag(reference.tag))
                {
                    obj.GetComponent<StudioEventEmitter>().setIntensity(intensite + dIntensite);
                    break;
                }
            }
        }
    }
    public float getIntensite()
    {
        return intensite;
    }


}

