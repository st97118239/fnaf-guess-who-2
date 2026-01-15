using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerFlags _playerFlags;
    public static PlayerFlags playerFlags
    {
        get => _playerFlags;
        set
        {
            //Debug.Log(value.HasFlag(PlayerFlags.CanInteract) ? "CanInteract is on" : "CanInteract is off");

            _playerFlags = value;
        }
    }
    public static Menu currentMenu;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private CameraManager cameraManager;

    [SerializeField] private ObjMouseHover folder;

    private void Start()
    {
        playerFlags = playerFlags.AddFlag(PlayerFlags.CanInteract);
    }

    private void OnBack()
    {
        if (currentMenu != Menu.None && PlayerFlagsExtensions.HasFlag(playerFlags, PlayerFlags.CanInteract))
            cameraManager.Back();
    }

    private void OnOpenFolder()
    {
        if (currentMenu == Menu.None && PlayerFlagsExtensions.HasFlag(playerFlags, PlayerFlags.CanInteract))
            folder.Open();
    }
}
