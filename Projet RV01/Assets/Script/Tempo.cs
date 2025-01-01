using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using CustomAudio;

public class Tempo : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The reference to the action of Trigger (gachette clic right).")]
    InputActionReference Trigger;
    public GameObject tempoObject; //objet tempo
    private GameObject tempoCapsule;
    public Transform controller; // transform du controller qu'on utilise
    public Camera camera; // main camera du XR Origin
    public float distanceFromUser = 0.2f; // distance à laquelle on met obj tempo par rapport a user
    private float tempo; // valeur du tempo

    private bool isActive = false; //  indique si tempoObject est actif dans la scène
    private bool timerRunning = false; // indique si le timer est en cours d'exécution
    private float timer = 0f;
    private int counter = 0;

    private void Start()
    {
        tempoCapsule = GameObject.Find("Tempo Capsule");
        tempoCapsule.SetActive(false);
    }

    // timer
    void Update()
    {
        if (timerRunning)
        {
            timer += Time.deltaTime;
        }

        // raycast pour collisions entre rayon du controller et l'obj tempo
        if (isActive)
        {

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
        Trigger.action.Disable(); // desactiver action

    }

    // quand on trigger la gachette, si l'obj tempo pas actif : activer l'objet tempo sinon arreter la mesure du tempo
    private void OnTrigger(InputAction.CallbackContext context)
    {
        Debug.Log("OnTrigger");

        if (context.performed) // verif que l'action est exécutée (et pas seulement commencée)
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

    // activer l'objet tempo sur la scene
    void ActivateTempoObject()
    {
        if (camera != null)
        {
            // place tempoObject devant user
            Vector3 positionTempo = camera.transform.position + camera.transform.forward * distanceFromUser;
            tempoObject.transform.position = positionTempo;

            tempoObject.SetActive(true);
            tempoCapsule.SetActive(true);
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
            tempoCapsule.SetActive(false);
            isActive = false;
            timerRunning = false;

            tempo = counter / timer * 60 ;
            Debug.Log("Tempo: " + tempo);
            AudioManager am = FindObjectOfType<AudioManager>();
            am.setTempo(tempo);
        }

        return 0.0f; // bof voir si ca pose pas pb
    }

    // gère collision entre le rayon du controller et tempoObject
    public void HandleObjectCollision()
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






