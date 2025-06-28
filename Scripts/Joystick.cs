using UnityEngine;
using UnityEngine.EventSystems; // So Unity knows about UI touches
using UnityEngine.UI; // So Unity knows about Image components

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public RectTransform joystickBackground; // Drag your joystick's background image here
    public RectTransform joystickHandle;     // Drag your joystick's handle image here

    [Range(0f, 2f)] public float handleRange = 1f; // How far the handle can move
    public float deadZone = 0.1f;                  // Ignore tiny wiggles near the center

    public Vector2 Direction { get; private set; } = Vector2.zero; // This is the direction other scripts will read!

    private Vector2 initialHandleLocalPosition; // Where the handle sits when not touched
    private Vector2 inputVector; // Internal variable for raw touch input

    void Awake()
    {
        // Just some helper code to make sure things are linked if you forget to drag them
        if (joystickBackground == null) joystickBackground = GetComponent<RectTransform>();
        if (joystickHandle == null && transform.childCount > 0) joystickHandle = transform.GetChild(0).GetComponent<RectTransform>();

        if (joystickHandle != null) initialHandleLocalPosition = joystickHandle.anchoredPosition;
    }

    // When you press down on the joystick
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Start dragging right away
    }

    // As you drag your finger on the joystick
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        // Figure out where your touch is relative to the joystick background
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBackground, eventData.position, eventData.pressEventCamera, out position))
        {
            OnPointerUp(eventData); // If you drag off the joystick, release it
            return;
        }

        // Normalize the position (make it a value between -1 and 1)
        position.x = (position.x / (joystickBackground.sizeDelta.x / 2f));
        position.y = (position.y / (joystickBackground.sizeDelta.y / 2f));
        inputVector = new Vector2(position.x, position.y);
        inputVector = Vector2.ClampMagnitude(inputVector, 1f); // Don't let it go past the edge of the circle

        // Apply dead zone (ignore small, accidental movements)
        if (inputVector.magnitude < deadZone)
        {
            inputVector = Vector2.zero;
        }

        // Move the joystick handle visually on screen
        joystickHandle.anchoredPosition = new Vector2(inputVector.x * joystickBackground.sizeDelta.x / 2f * handleRange,
                                                      inputVector.y * joystickBackground.sizeDelta.y / 2f * handleRange);

        Direction = inputVector; // Update the direction for other scripts
    }

    // When you lift your finger from the joystick
    public void OnPointerUp(PointerEventData eventData)
    {
        // Snap the handle back to the center
        if (joystickHandle != null) joystickHandle.anchoredPosition = initialHandleLocalPosition;
        Direction = Vector2.zero; // Stop providing input
        inputVector = Vector2.zero;
    }
}