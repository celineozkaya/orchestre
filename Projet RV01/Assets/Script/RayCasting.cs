using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class RayCasting : MonoBehaviour
{
    public Camera camera;
    private LayerMask lookableLayer; // instruments que l'on dirige
    private GameObject currentObject=null; // objet observé (et donc highlighté)
    private GameObject selectedObject; // objet sélectionné (qu'on controle)
    private Material originalMaterial; //matériau d'origine temporaire
    private Material tempMaterial; // matériau temporaire utilisé pour le highlight
    public Color highlightColor = Color.yellow; // couleur de l'effet d'émission

    private float offset = 0.0f;
    private const int rayCastDistance = 100;
  
    // Start is called before the first frame update
    void Start(){
        lookableLayer = LayerMask.GetMask("Lookable");
    }

    // Update is called once per frame
    void FixedUpdate(){
        // Ray ray = new Ray(camera.transform.position, camera.transform.forward); // verifier 

        Debug.Log(" cam = " + camera.transform.forward);
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        Debug.Log("Ray origin: " + ray.origin + ", Ray direction: " + ray.direction);
        Debug.DrawRay(ray.origin, ray.direction * rayCastDistance, Color.red);
        RaycastHit hit;

        // Lancer un rayon depuis le regard du casque
        if (Physics.Raycast(ray, out hit, rayCastDistance)){
            GameObject hitObject = hit.collider.gameObject; // l'objet détecté par le Raycast à l'instant T

            if (hitObject != currentObject){
                // Réinitialiser l'objet précédent
                if (currentObject != null && currentObject != selectedObject){
                    // retirer le highlight à currentObject
                    ResetEmission(currentObject);
                }

                // Appliquer le highlight sur le nouvel objet observé
                if (hitObject != selectedObject){
                    // appliquer le highlight à hitObject
                    SetEmission(hitObject,highlightColor);
                }

                currentObject = hitObject;
            }
        }
        else{
            // Réinitialiser l'objet précédent si le rayon ne touche rien
            if (currentObject != null && currentObject != selectedObject){
                // retirer le highlight de currentObject
                ResetEmission(currentObject);
            }
            currentObject = null;
        }
        
    }

    // ajouter le highlight à l'objet obj
    private void SetEmission(GameObject obj, Color emissionColor){
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
        
    }

    // retirer le hightlight de l'objet obj
    private void ResetEmission(GameObject obj){
        Renderer renderer = obj.GetComponent<Renderer>(); // // recup render
        if (renderer != null && originalMaterial != null){
            renderer.material = originalMaterial; // re applique le matériau d'origine
        }
    }


    // si clic sur side clic = grab move (> grip)
    // grip donne un float
    void onSelectValue(){
        if (currentObject != null){
            if (currentObject == selectedObject){
                // Désélectionner l'objet
                // ResetEmission(selectedObject);
                selectedObject = null;
            }
            else{
                // Sélectionner le nouvel objet
                if (selectedObject != null){
                    // ResetEmission(selectedObject);
                }
                selectedObject = currentObject;
            }
        }
    }
}
