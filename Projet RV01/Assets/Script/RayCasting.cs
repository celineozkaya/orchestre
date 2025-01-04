using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.HighDefinition;


public class RayCasting : MonoBehaviour
{

    [SerializeField]
    [Tooltip("The reference to the action of Grab (grab clic right).")]
    InputActionReference Grab;

    public Camera camera;
    private LayerMask lookableLayer; // layer designant les instruments que l'on peut diriger
    private LayerMask projecteurLayer; // layer designant les spotlights
    private GameObject currentObject=null; // objet observé (et donc highlighté)
    private GameObject selectedObject; // objet sélectionné (qu'on controle)
    public float minIntensity = 25f; // valeur min de l'intensite de l'émission des spots sur les instruments
    public float maxIntensity = 30f; // valeur min de l'intensite de l'émission des spots sur les instruments
    private float offset = 0.0f;
    private const int rayCastDistance = 1000;
  
    // Start is called before the first frame update
    void Start(){
        lookableLayer = LayerMask.GetMask("Lookable");
        projecteurLayer = LayerMask.GetMask("Projecteur");
    }

    // Update is called once per frame
    void FixedUpdate(){
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.red);
        RaycastHit hit;

        // lancer un rayon depuis le casque
        if (Physics.Raycast(ray.origin, ray.direction, out hit, rayCastDistance, lookableLayer)){
            GameObject hitObject = hit.collider.gameObject; // l'objet détecté par le Raycast à l'instant T
            if (hitObject != currentObject){
                // réinitialiser l'objet précédent
                if (currentObject != null && currentObject != selectedObject){
                    // retirer le highlight à currentObject
                    if (selectedObject == null)
                    {
                        AdjustSpotlightIntensity(currentObject.tag, minIntensity); // baisser intensité du spot
                    }
                }
                // appliquer le highlight sur le nouvel objet observé
                if (hitObject != selectedObject){
                    Debug.Log("hitObject.tag : " + hitObject.tag);
                    if (selectedObject == null)
                    {
                        AdjustSpotlightIntensity(hitObject.tag, maxIntensity); // augmenter intensité du spot
                    }
                }
                currentObject = hitObject;
            }
        }
        else{
            // réinitialiser l'objet précédent si le rayon ne touche rien
            if (currentObject != null)
            {
                // retirer le highlight de currentObject
                if (selectedObject == null)
                {
                    AdjustSpotlightIntensity(currentObject.tag, minIntensity);
                }
                Debug.Log("currentObject.tag : "+ currentObject.tag);
            }
            currentObject = null;
        }
    }

    // retourne une liste contenant les gamesobject (spotlights) associés 
    // a l'instrument selectionne en fonction du tag et du layer
    public static List<GameObject> FindGameObjectsWithLayerAndTag(int layer, string tag)
    {
        GameObject[] goArray = Object.FindObjectsOfType<GameObject>();
        List<GameObject> returnArray = new List<GameObject> ();

        foreach (GameObject go in goArray)
        {
            if (go.layer == layer && go.CompareTag(tag))
            {
                returnArray.Add(go);
            }
        }
        return returnArray;
    }

    // permet de modifier l'intensité (targetIntensity) du spotlight des instruments taggués par tag
    private void AdjustSpotlightIntensity(string tag, float targetIntensity)
    {
        List<GameObject> spots = FindGameObjectsWithLayerAndTag(7, tag);
        foreach (GameObject spot in spots)
        {
            spot.GetComponent<HDAdditionalLightData>().SetIntensity(targetIntensity, LightUnit.Ev100);
        }
    }

    private void OnEnable()
    {
        Grab.action.Enable(); // activer action
        Grab.action.performed += OnGrab; // listener pour le OnGrab

    }

    private void OnDisable()
    {
        Grab.action.performed -= OnGrab; // retirer listener pour le OnGrab
        Grab.action.Disable(); // deactiver action

    }

    
    // au clic sur grip, le groupe est selectionné/ deselectionné selon le contexte
    void OnGrab(InputAction.CallbackContext context)
    {
        if (selectedObject != null) // si un groupe est selectionné
        {
            // désélectionner l'objet
            AdjustSpotlightIntensity(selectedObject.tag, minIntensity);
            selectedObject = null;
        }
        else if (currentObject != null) // sinon si je regarde un grp
        {
            // sélectionner le nouvel objet
            selectedObject = currentObject;
            AdjustSpotlightIntensity(selectedObject.tag, maxIntensity);
        }
    }

    // getter pour le groupe d'instrument selectionné
    public GameObject getSelectedObject()
    {
        return selectedObject;
    }



}
