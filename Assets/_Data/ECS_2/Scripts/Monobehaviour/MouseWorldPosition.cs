using UnityEngine;
using UnityEngine.InputSystem;

public class MouseWorldPosition : MonoBehaviour
{
    public static MouseWorldPosition Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetPosition()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue(); ;
        Ray mouseCameraRay = Camera.main.ScreenPointToRay(mousePos);
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        if(plane.Raycast(mouseCameraRay, out float distance))
        {
            return mouseCameraRay.GetPoint(distance);
        }
        else
        {
            return Vector3.zero;
        }
    }
}
