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

    //ccc
    public Camera camera;
    private LayerMask lookableLayer; // instruments que l'on dirige
    private LayerMask projecteurLayer; // 

    private GameObject currentObject=null; // objet observé (et donc highlighté)
    private GameObject selectedObject; // objet sélectionné (qu'on controle)
    private Material originalMaterial; //matériau d'origine temporaire
    private Material tempMaterial; // matériau temporaire utilisé pour le highlight
    //public Color highlightColor = Color.yellow; // couleur de l'effet d'émission
    public float minIntensity = 25f; // valeur min de l'intentsité de l'émission des spot sur les instruments
    public float maxIntensity = 30f; // valeur min de l'intentsité de l'émission des spot sur les instruments

    private float offset = 0.0f;
    private const int rayCastDistance = 200;
  
    // Start is called before the first frame update
    void Start(){
        lookableLayer = LayerMask.GetMask("Lookable");
        projecteurLayer = LayerMask.GetMask("Projecteur");
        //Debug.Log("start");
    }

    // Update is called once per frame
    void FixedUpdate(){
        //Debug.Log("fixed update");
        //Debug.Log(" cam = " + camera.transform.forward);
        // Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        //Debug.Log("Ray origin: " + ray.origin + ", Ray direction: " + ray.direction);
        Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.red);
        RaycastHit hit;

        // lancer un rayon depuis le regard du casque
        if (Physics.Raycast(ray.origin, ray.direction, out hit, rayCastDistance, lookableLayer)){
            GameObject hitObject = hit.collider.gameObject; // l'objet détecté par le Raycast à l'instant T

            if (hitObject != currentObject){

                // réinitialiser l'objet précédent
                if (currentObject != null && currentObject != selectedObject){
                //if (currentObject != null){


                        // retirer le highlight à currentObject
                        // ResetEmission(currentObject);
                        AdjustSpotlightIntensity(currentObject.tag, minIntensity); // baisser intensité du spot
                }
                // appliquer le highlight sur le nouvel objet observé
                if (hitObject != selectedObject){
                    // appliquer le highlight à hitObject
                    // SetEmission(hitObject,highlightColor);
                    Debug.Log("hitObject.tag : " + hitObject.tag);

                    AdjustSpotlightIntensity(hitObject.tag, maxIntensity); // augmenter intensité du spot
                }

                currentObject = hitObject;
            }
        }
        else{
            // réinitialiser l'objet précédent si le rayon ne touche rien
            //if (currentObject != null && currentObject != selectedObject){
            if (currentObject != null)
            {

                    // retirer le highlight de currentObject
                    // ResetEmission(currentObject);
                    //Debug.Log("ici3");

                AdjustSpotlightIntensity(currentObject.tag, minIntensity);
                Debug.Log("currentObject.tag : "+ currentObject.tag);
            }
            currentObject = null;
        }
        
    }

    // on veut edit la lumiere du spotlight (spotlight > emission > augmenter intensité (de 25Ev à 30Ev)
    // ajouter le highlight à l'objet obj
/*    private void SetEmission(GameObject obj, Color emissionColor){
        Renderer renderer = obj.GetComponent<Renderer>(); // recup render
        if (renderer != null){
            Material material = renderer.material; 
            if (tempMaterial == null || material != tempMaterial){
                originalMaterial = material;
                tempMaterial = new Material(originalMaterial); // dupplique le material pour pas changer l'original
                tempMaterial.EnableKeyword("_EMISSION");
                tempMaterial.SetColor("_EmissionColor", emissionColor);
                renderer.material = tempMaterial; // applique le matériau temporaire
            }
        }
        
    }*/

    // retirer le hightlight de l'objet obj
/*    private void ResetEmission(GameObject obj){
        Renderer renderer = obj.GetComponent<Renderer>(); // // recup render
        if (renderer != null && originalMaterial != null){
            renderer.material = originalMaterial; // re applique le matériau d'origine
        }
    }*/

    public static GameObject FindGameObjectWithLayerAndTag(int layer, string tag)
    {
        GameObject[] goArray = Object.FindObjectsOfType<GameObject>();

        foreach (GameObject go in goArray)
        {
            if (go.layer == layer && go.CompareTag(tag))
            {
                return go;
            }
        }

        return null;
    }

    private void AdjustSpotlightIntensity(string tag, float targetIntensity)
    {
        GameObject spot = FindGameObjectWithLayerAndTag(7, tag);
        Debug.Log($"[AdjustSpotlightIntensity] Tag: {tag}, TargetIntensity: {targetIntensity}");
        Debug.Log("nom spot : " + spot.name);

        spot.GetComponent<HDAdditionalLightData>().SetIntensity(targetIntensity, LightUnit.Ev100);

        
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

    
    // si clic sur side clic = grab move (> grip)
    // grip donne un float
  void OnGrab(InputAction.CallbackContext context)
    {
        Debug.Log("dans OnGrab");

        if (currentObject != null)
        {
            if (currentObject == selectedObject)
            {
                Debug.Log("if currentObject == selectedObject");
                // Désélectionner l'objet
                AdjustSpotlightIntensity(selectedObject.tag, minIntensity);
                Debug.Log("Désélection de l'objet : " + selectedObject.tag);
                selectedObject = null;
            }
            else
            {
                Debug.Log("else - Sélection du nouvel objet");

                // Si un autre objet était sélectionné, le réinitialiser
                if (selectedObject != null)
                {
                    Debug.Log("Réinitialisation de l'objet sélectionné précédent : " + selectedObject.tag);
                    AdjustSpotlightIntensity(selectedObject.tag, minIntensity);
                }

                // Sélectionner le nouvel objet
                selectedObject = currentObject;
                AdjustSpotlightIntensity(selectedObject.tag, maxIntensity);
                Debug.Log("Nouvel objet sélectionné : " + selectedObject.tag);
            }
        }
    }

    public GameObject getSelectedObject()
    {
        return selectedObject;
    }



}
