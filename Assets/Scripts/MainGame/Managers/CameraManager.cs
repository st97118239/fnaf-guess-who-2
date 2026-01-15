using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Animator animator;

    public ObjMouseHover folder;
    public ObjMouseHover mainBoard;
    public ObjMouseHover characterBoard;

    [SerializeField] private ObjMouseHover objCamIsAt;
    [SerializeField] private ObjMouseHover prevObjCamIsAt;

    private bool isLookingAtObj;
    private bool isMoving;
    private bool wasOnMainBoard;
    private bool wasOnCharBoard;

    public void Back()
    {
        if (!isLookingAtObj || !PlayerFlagsExtensions.HasFlag(InputManager.playerFlags, PlayerFlags.CanInteract)) return;

        if (wasOnMainBoard)
            mainBoard.Open();
        else if (wasOnCharBoard)
            characterBoard.Open();
        else
            AnimateCam(true);
    }

    public void PlayMoveAnim(ObjMouseHover givenComponent)
    {
        if (isLookingAtObj)
        {
            prevObjCamIsAt = objCamIsAt;
            objCamIsAt.CanvasBlocker(true);
        }

        if (InputManager.currentMenu != Menu.None)
        {
            switch (objCamIsAt.menu)
            {
                case Menu.MainBoard:
                    wasOnMainBoard = true;
                    wasOnCharBoard = false;
                    break;
                case Menu.CharacterBoard:
                    wasOnMainBoard = false;
                    wasOnCharBoard = true;
                    break;
                default:
                    wasOnMainBoard = false;
                    wasOnCharBoard = false;
                    break;
            }
        }
        objCamIsAt = givenComponent;
        AnimateCam(false);
    }

    private void AnimateCam(bool isGoingBack)
    {
        if (!objCamIsAt)
        {
            Debug.LogError("objCamIsAt is not set");
            return;
        }

        animator.SetTrigger(objCamIsAt.menu.ToString());
        if (isGoingBack)
        {
            InputManager.currentMenu = Menu.None;
            isLookingAtObj = false;
            objCamIsAt.CanvasBlocker(true);
        }
        else
        {
            InputManager.currentMenu = objCamIsAt.menu;
            isLookingAtObj = true;
            objCamIsAt.CanvasBlocker(false);
        }

    }

    public void ToggleInteractionWhileAnimation()
    {
        if (!isMoving)
        {
            isMoving = true;
            DisableInteraction();
        }
        else
        {
            isMoving = false;
            EnableInteraction();

            if (prevObjCamIsAt)
            {
                prevObjCamIsAt.Unselect();
                prevObjCamIsAt = null;
            }

            if (InputManager.currentMenu != Menu.None) return;
            objCamIsAt.Unselect();
            objCamIsAt = null;
        }
    }

    public void DisableInteraction()
    {
        InputManager.playerFlags = InputManager.playerFlags.RemoveFlag(PlayerFlags.CanInteract);
    }

    public void EnableInteraction()
    {
        InputManager.playerFlags = InputManager.playerFlags.AddFlag(PlayerFlags.CanInteract);
    }
}
