using UnityEngine;
using UnityEngine.Events;

public class ObjMouseHover : MonoBehaviour
{
    [SerializeField] private UnityEvent pressedObj;
    [SerializeField] private Outline outline;

    [SerializeField] private GameObject canvasBlocker;
    public Transform camPoint;
    public float camMoveSpeed;
    public float camTurnSpeed;
    public Menu menu;

    private bool isSelected;

    private void OnMouseOver()
    {
        if (isSelected || !PlayerFlagsExtensions.HasFlag(InputManager.playerFlags, PlayerFlags.CanInteract)) return;

        outline.enabled = true;

        if (!Input.GetMouseButtonDown(0)) return;
        Open();
    }

    public void Open()
    {
        isSelected = true;
        pressedObj?.Invoke();
    }

    private void OnMouseExit()
    {
        if (isSelected) return;

        outline.enabled = false;
    }

    public void Unselect()
    {
        isSelected = false;
    }

    public void CanvasBlocker(bool toggle)
    {
        if (!canvasBlocker) return;
        canvasBlocker.SetActive(toggle);
    }
}
