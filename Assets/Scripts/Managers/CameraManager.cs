using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Transform defaultCamPoint;
    [SerializeField] private float defaultCamMoveSpeed;
    [SerializeField] private float defaultCamTurnSpeed;

    [SerializeField] private ObjMouseHover objCamIsAt;

    private bool isLookingAtObj;

    public void Back()
    {
        if (!isLookingAtObj || !PlayerFlagsExtensions.HasFlag(InputManager.playerFlags, PlayerFlags.CanInteract)) return;
        InputManager.playerFlags = InputManager.playerFlags.RemoveFlag(PlayerFlags.CanInteract);
        StartCoroutine(MoveCam(defaultCamPoint.position, defaultCamPoint.rotation, objCamIsAt.camMoveSpeed, objCamIsAt.camTurnSpeed, true));
    }

    public void MoveCamToScreen(ObjMouseHover givenComponent)
    {
        objCamIsAt = givenComponent;
        StartCoroutine(MoveCam(objCamIsAt.camPoint.position, objCamIsAt.camPoint.rotation, objCamIsAt.camMoveSpeed, objCamIsAt.camTurnSpeed, false));
    }

    private IEnumerator MoveCam(Vector3 posToMoveTo, Quaternion angleToRotateTo, float camMoveSpeed, float camTurnSpeed, bool shouldUnselect)
    {
        while (true)
        {
            InputManager.playerFlags = InputManager.playerFlags.RemoveFlag(PlayerFlags.CanInteract);

            cam.transform.SetPositionAndRotation(
                Vector3.MoveTowards(cam.transform.position, posToMoveTo, camMoveSpeed * Time.deltaTime),
                Quaternion.RotateTowards(cam.transform.rotation, angleToRotateTo, camTurnSpeed * Time.deltaTime)
                );

            if (cam.transform.position == posToMoveTo && cam.transform.rotation == angleToRotateTo)
            {
                if (shouldUnselect)
                {
                    isLookingAtObj = false;
                    InputManager.currentMenu = Menu.None;
                    objCamIsAt.CanvasBlocker(true);
                    objCamIsAt.Unselect();
                }
                else
                {
                    isLookingAtObj = true;
                    InputManager.currentMenu = objCamIsAt.menu;
                    objCamIsAt.CanvasBlocker(false);
                }

                InputManager.playerFlags = InputManager.playerFlags.AddFlag(PlayerFlags.CanInteract);

                yield break;
            }

            yield return Time.deltaTime;
        }
    }
}
