using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardManager : MonoBehaviour
{
    [SerializeField] private CameraManager camMngr;
    [SerializeField] private FolderManager fldrMngr;
    [SerializeField] private CharactersLoader charactersLoader;
    [SerializeField] private Polaroid polaroidPrefab;
    [SerializeField] private Transform polaroidParent;

    [SerializeField] private int characterCount;
    [SerializeField] private Polaroid[] polaroids;

    [SerializeField] private Button backButton;
    [SerializeField] private Button reloadButton;

    private int openPackIdx;

    private void Start() => SpawnPolaroids();

    private void SpawnPolaroids()
    {
        if (polaroids != null)
        {
            foreach (Polaroid polaroid in polaroids)
                Destroy(polaroid.gameObject);
        }

        polaroids = new Polaroid[characterCount];

        for (int i = 0; i < characterCount; i++) 
            polaroids[i] = Instantiate(polaroidPrefab, polaroidParent);

        LoadPacks();
    }

    private void LoadPacks()
    {
        openPackIdx = -1;
        backButton.interactable = false;
        UnloadPolaroids();
        int max = CharactersLoader.packs.Count;
        for (int i = 0; i < max; i++)
        {
            if (i < max)
                polaroids[i].LoadPack(i, this);
        }
        reloadButton.interactable = true;
    }

    private void UnloadPolaroids()
    {
        foreach (Polaroid polaroid in polaroids) 
            polaroid.Unload();
    }

    private void LoadChars()
    {
        reloadButton.interactable = false;
        UnloadPolaroids();

        string[] characterPaths = CharactersLoader.GetCharacterPathsFromPack(openPackIdx);

        int max = characterPaths.Length;
        for (int i = 0; i < max; i++)
        {
            string character = characterPaths[i];
            if (character == string.Empty)
            {
                Debug.LogError("Character could not be loaded.");
                continue;
            }

            if (i < max)
                polaroids[i].LoadChar(openPackIdx, i, this);
        }
        backButton.interactable = true;
    }

    public void OpenPack(int packIdx)
    {
        openPackIdx = packIdx;
        LoadChars();
    }

    public void OpenFolder(int packIdx, int charIdx)
    {
        camMngr.folder.OpenCharacterFolder(packIdx, charIdx);
    }

    public void BackButton()
    {
        if (openPackIdx > -1)
            LoadPacks();
    }

    public void ReloadButton()
    {
        reloadButton.interactable = false;

        UnloadPolaroids();

        CharactersLoader.ReloadCharacters();

        SpawnPolaroids();
    }
}
