using UnityEngine;

public class TouchRelocateObject : MonoBehaviour
{
    public GameObject objectToMove; // Assign your Turbine Prefab here in the Inspector
    public LayerMask clickableLayers; // Optional: Specify which layers can be clicked

    void Update()
    {
        // For PC/Mac (Mouse Input)
        if (Input.GetMouseButtonDown(0)) // Checks if the left mouse button was clicked
        {
            HandleInput(Input.mousePosition);
        }

        // For Mobile (Touch Input)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) // Checks if a touch began
        {
            HandleInput(Input.GetTouch(0).position);
        }
    }

    void HandleInput(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // Perform the raycast
        // We're casting into the scene to find a point on a collider
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickableLayers))
        {
            // If the ray hits something, relocate the objectToMove to the hit point
            if (objectToMove != null)
            {
                objectToMove.transform.position = hit.point;
                Debug.Log("Object moved to: " + hit.point);
            }
            else
            {
                Debug.LogWarning("objectToMove is not assigned in the Inspector!");
            }
        }
        else
        {
            Debug.Log("No object hit by raycast at this position.");
        }
    }
}