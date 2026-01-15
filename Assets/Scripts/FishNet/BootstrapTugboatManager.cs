using UnityEngine;

public class BootstrapTugboatManager : MonoBehaviour
{
    [SerializeField] private GameObject bootstrapCanvas;
    [SerializeField] private GameObject bootstrapEventSystem;

    private void Start()
    {
        Settings.isUsingSteamworks = false;
        Debug.Log("Started game with Tugboat Transport");
    }

    public void RemoveBootstrapGarbage()
    {
        Destroy(bootstrapCanvas);
        Destroy(bootstrapEventSystem);
    }
}
