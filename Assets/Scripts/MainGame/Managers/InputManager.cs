using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static PlayerFlags playerFlags;
    public static Menu currentMenu;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private ObjMouseHover folder;

    [SerializeField] private Button backButton;

    private static InputManager instance;

    private void Awake() => instance = this;

    private void Start()
    {
        playerFlags = playerFlags.AddFlag(PlayerFlags.CanInteract);
        CheckBackButton();
    }

    public void BackButton()
    {
        OnBack();
    }

    private void OnBack()
    {
        if (currentMenu != Menu.None && PlayerFlagsExtensions.HasFlag(playerFlags, PlayerFlags.CanInteract))
            cameraManager.Back();

        CheckBackButton();
    }

    private void OnOpenFolder()
    {
        if (currentMenu == Menu.None && PlayerFlagsExtensions.HasFlag(playerFlags, PlayerFlags.CanInteract))
            folder.Open();
    }

    public static void CheckBackButton()
    {
        if (currentMenu != Menu.None && PlayerFlagsExtensions.HasFlag(playerFlags, PlayerFlags.CanInteract))
            instance.backButton.interactable = true;
        else
            instance.backButton.interactable = false;
    }
}
