using UnityEngine;

public class ARCameraZoom : MonoBehaviour
{
    [Header("Zoom Target & Speed")]
    public Transform targetObject; // Drag your AR Session Origin here
    public float zoomSpeed = 0.05f; // How fast the zoom happens (adjust for feel)

    [Header("Zoom Distance Limits")]
    public float minDistance = 0.5f; // Closest you can zoom (e.g., 50cm from target)
    public float maxDistance = 5f;   // Furthest you can zoom (e.g., 5 meters from target)

    private Vector2 previousTouch1Pos;
    private Vector2 previousTouch2Pos;
    private float initialDistanceBetweenTouches;
    private float initialCameraDistanceFromTarget; // Store camera's initial distance from target

    void Update()
    {
        // Basic check to ensure targetObject is assigned
        if (targetObject == null)
        {
            Debug.LogWarning("ARCameraZoom: Target Object (AR Session Origin) not assigned!");
            return;
        }

        // --- Detect Two-Finger Touch Input ---
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // --- Phase Began: When the two fingers first touch down ---
            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                previousTouch1Pos = touch1.position;
                previousTouch2Pos = touch2.position;
                initialDistanceBetweenTouches = Vector2.Distance(touch1.position, touch2.position);
                initialCameraDistanceFromTarget = Vector3.Distance(transform.position, targetObject.position);
            }
            // --- Phase Moved: As the two fingers are dragged ---
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                Vector2 currentTouch1Pos = touch1.position;
                Vector2 currentTouch2Pos = touch2.position;

                float currentDistanceBetweenTouches = Vector2.Distance(currentTouch1Pos, currentTouch2Pos);

                // Calculate how much the pinch/spread has changed since the touches began
                // A positive pinchAmount means fingers are spreading (zoom in/closer)
                // A negative pinchAmount means fingers are pinching (zoom out/further)
                float pinchAmount = (currentDistanceBetweenTouches - initialDistanceBetweenTouches) * zoomSpeed;

                // Calculate the new desired distance from the target object
                // We subtract pinchAmount because spreading fingers (positive pinchAmount)
                // should make the camera move *closer* (smaller distance).
                float newDesiredDistance = initialCameraDistanceFromTarget - pinchAmount;

                // Clamp the distance within the defined min/max bounds
                newDesiredDistance = Mathf.Clamp(newDesiredDistance, minDistance, maxDistance);

                // Move the AR Camera (this GameObject) towards/away from the targetObject
                // 1. Get the direction from the target to the camera
                Vector3 directionFromTarget = (transform.position - targetObject.position).normalized;
                // 2. Set the camera's new position
                transform.position = targetObject.position + directionFromTarget * newDesiredDistance;

                // Update initial distance for next frame's relative movement
                initialCameraDistanceFromTarget = newDesiredDistance; // Update to the new distance
                initialDistanceBetweenTouches = currentDistanceBetweenTouches; // Update for continuous dragging
            }
        }
    }
}