using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class RaycastUI : MonoBehaviour
{
    public Camera mainCamera; // Drag Main Camera here in Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            // Ray from center of screen
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // 1. Check UI first
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = new Vector2(Screen.width / 2, Screen.height / 2); // center screen

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                Debug.Log("UI Hit: " + results[0].gameObject.name);
                ExecuteEvents.Execute(results[0].gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                return;
            }


            // 2. If no UI hit, check physics objects
            if (Physics.Raycast(ray, out hit, 100f))
            {
                Debug.Log("Hit 3D object: " + hit.collider.gameObject.name);
            }
        }
    }
}
