using UnityEngine;

public class CameraRaycast : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask layerMask;


    private bool isOverObj;

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 10, layerMask))
        {
            if (!isOverObj)
            {
                isOverObj = true;
            }
        }
        else
        {
            if (isOverObj)
            {
                isOverObj = false;
            }
        }
    }
}
