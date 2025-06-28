using UnityEngine;

public class ModelRotator : MonoBehaviour
{
    public Joystick joystick;         // Drag your JoystickBackground (with Joystick.cs) here
    public float rotationSpeed = 100f; // How fast your model spins (adjust this!)

    public Transform targetModel;     // Drag your actual 3D model's main object here

    void Start()
    {
        // Try to find the joystick if you forget to drag it (helpful!)
        if (joystick == null)
        {
            joystick = FindObjectOfType<Joystick>();
            if (joystick == null) {
                Debug.LogError("ModelRotator: Joystick not found or assigned! Rotation won't work.", this);
                enabled = false; // Turn off this script if no joystick
                return;
            }
        }

        // If no model is dragged, assume this script is on the model itself
        if (targetModel == null)
        {
            targetModel = this.transform;
        }
    }

    void Update()
    {
        Vector2 joystickDirection = joystick.Direction;

        // Only rotate if the joystick is being moved
        if (joystickDirection.magnitude > 0)
        {
            // Rotate around the Y-axis (like spinning on a turntable)
            // joystickDirection.x is for left/right movement
            float rotateY = joystickDirection.x * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.up, rotateY, Space.World); // Use Space.World for global Y-axis spin

            // Rotate around the X-axis (like tilting forward/backward)
            // joystickDirection.y is for up/down movement
            // We use -joystickDirection.y because moving the joystick UP usually means tilting the *view* UP,
            // which often feels like tilting the *object* BACKWARDS (negative pitch). Adjust sign if it feels wrong!
            float rotateX = -joystickDirection.y * rotationSpeed * Time.deltaTime;
            targetModel.Rotate(Vector3.right, rotateX, Space.Self); // Use Space.Self for local X-axis tilt
        }
    }
}