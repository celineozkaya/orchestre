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

    public Transform controller; // contr�leur
    public float minHeight = -2.0f; // hauteur minimale
    public float maxHeight = 2.0f;  // hauteur maximale

    private float initialHeight; // hauteur de d�part du contr�leur
    private float intensite; 

    void Start()
    {
        if (controller != null)
        {
            initialHeight = controller.position.y; //  hauteur initiale du contr�leur
        }
    }

    void Update()
    {
        if (controller != null)
        {
            // calcul diff�rence de hauteur depuis la position initiale
            float deltaHeight = controller.position.y - initialHeight;

            // normalisation de la diff�rence pour qu'elle soit dans l'intervalle [-2, 2]
            intensite = Mathf.Clamp(deltaHeight, minHeight, maxHeight);

            Debug.Log("Controller Height (Normalized): " + intensite);
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
        
        // calcul diff�rence de hauteur depuis la position initiale
        float deltaHeight = controller.position.y - initialHeight;

        // normalisation de la diff�rence pour qu'elle soit dans l'intervalle [-2, 2]
        intensite = Mathf.Clamp(deltaHeight, minHeight, maxHeight);

        Debug.Log("Controller Height (Normalized): " + intensite);
    }
    public float getIntensite()
    {
        return intensite;
    }


}

