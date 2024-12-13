using FMODUnity;
using System.Collections;
using System.Collections.Generic;
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
    public float minHeight = -0.1f; // hauteur minimale
    public float maxHeight = 0.1f;  // hauteur maximale

    private float initialHeight; // hauteur de départ du contrôleur
    private float intensite = 1;
    private float dIntensite = 0;

    private float originPositionController;

    private StudioEventEmitter[] studioEventEmitters;

    void Start()
    {
        if (controller != null)
        {
            initialHeight = controller.position.y; //  hauteur initiale du contrôleur
        }
        studioEventEmitters = FindObjectsOfType<StudioEventEmitter>();



    }

    void Update()
    {
        if (controller != null  && isActive)
        {
            // retourner une valeur entre 0 et 2  donc changer ca 
            // calcul différence de hauteur depuis la position initiale
            float deltaHeight = Mathf.Clamp(controller.position.y - originPositionController, minHeight, maxHeight);

            // normalisation de la différence pour qu'elle soit dans l'intervalle [0, 2]
            dIntensite = (deltaHeight - minHeight) / (maxHeight - minHeight) * 2.0f - 1.0f;
            Debug.Log("deltaHeight : " + deltaHeight);

            Debug.Log("dIntensite : " + dIntensite);

            foreach (StudioEventEmitter emitter in studioEventEmitters)
            {
                emitter.setIntensity(Mathf.Clamp(intensite + dIntensite, 0, 2));
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

        if (!isActive)
        {
            intensite = Mathf.Clamp(intensite + dIntensite, 0, 2);
        }
    }
    public float getIntensite()
    {
        return intensite;
    }


}

