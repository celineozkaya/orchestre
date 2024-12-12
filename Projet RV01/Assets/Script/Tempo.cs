using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class Tempo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action of Trigger (gachette clic right).")]
    InputActionReference Trigger;
    public GameObject tempoObject; //obj tempo 
    public Transform controller; // transform du controller conserné
    public Camera camera; // main camera du XR Origin
    public float distanceFromUser = 1.5f; // distance de user à laquelle placer l'obj tempo 
    public float rayLength = 5f;    // taille du rayon sortant du controlleur
    private float tempo; // tempo

    private bool isActive = false; //  indique si tempoObject est actif dans la scène
    private bool timerRunning = false; // indique si le timer est en cours d'exécution
    private float timer = 0f;
    private int counter = 0;

    // timer
    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

        // Perform raycast to detect collision with tempoObject.
        if (isActive)
        {
            Ray ray = new Ray(controller.position, controller.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                if (hit.collider.gameObject == tempoObject)
                {
                    HandleRayCollision();
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

        // clic gachette
        // when(traverse obj temp){
        // start timer
        // if(entre en contacte avec obj tempo){
        // count ++
        // }
        // 

        if (context.performed) // S'assurer que l'action est exécutée (et pas seulement commencée)
        {
            if (!isActive)
            {
                Debug.Log("appel de ActivateTempoObject");
                ActivateTempoObject();
            }
            else
            {
                Debug.Log("appel de StopTimer");
                StopTimer();
            }
        }
    }

    void ActivateTempoObject()
    {
        if (camera != null)
        {
            // place tempoObject devant user
            Vector3 positionTempo = camera.transform.position + camera.transform.forward * distanceFromUser;
            tempoObject.transform.position = positionTempo;

            tempoObject.SetActive(true);
            isActive = true;
            timer = 0f;
            counter = 0;
        }
    }

    // arrete le timer et retourne le tempo
    float StopTimer()
    {
        Debug.Log("dans StopTimer");

        if (isActive)
        {
            tempoObject.SetActive(false);
            isActive = false;
            timerRunning = false;

            tempo = counter / timer;
            Debug.Log("Tempo: " + tempo);
        }

        return 0.0f; // bof voir si ca pose pas pb
    }

    // gere collision entre le rayon du controller et tempoObject
    void HandleRayCollision()
    {
        if (!timerRunning)
        {
            timerRunning = true;
        }

        counter++;
        Debug.Log("counter : " + counter);
        // retour haptic (vibration à chaque passage dans l'objet tempo)
        HapticImpulsePlayer haptic = controller.GetComponent<HapticImpulsePlayer>();
        haptic.SendHapticImpulse(1, 0.1f, 30);

    }

    public float getTempo()
    {
        return tempo;
    }
}






