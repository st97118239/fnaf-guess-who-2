using UnityEngine;

public class CharacterBoardManager : MonoBehaviour
{
    [SerializeField] private CameraManager camMngr;
    [SerializeField] private CharactersLoader charactersLoader;
    [SerializeField] private Polaroid polaroidPrefab;
    [SerializeField] private Transform polaroidParent;

    [SerializeField] private int characterCount;
    [SerializeField] private Polaroid[] polaroids;

    private int openPackIdx;

    private void Start() => SpawnPolaroids();

    private void SpawnPolaroids()
    {
        polaroids = new Polaroid[characterCount];

        for (int i = 0; i < characterCount; i++) 
            polaroids[i] = Instantiate(polaroidPrefab, polaroidParent);

        LoadPacks();
    }

    private void LoadPacks()
    {
        openPackIdx = -1;
        UnloadPolaroids();
        int max = CharactersLoader.packs.Count;
        for (int i = 0; i < max; i++)
        {
            if (i < max)
                polaroids[i].LoadPack(i, this);
        }
    }

    private void UnloadPolaroids()
    {
        foreach (Polaroid polaroid in polaroids) 
            polaroid.Unload();
    }

    private void LoadChars()
    {
        UnloadPolaroids();
        int max = CharactersLoader.packs[openPackIdx].characters.Length;
        for (int i = 0; i < max; i++)
        {
            Character character = CharactersLoader.packs[openPackIdx].characters[i];
            if (character == null)
            {
                Debug.LogError("Character could not be loaded.");
                continue;
            }

            if (i < max)
                polaroids[i].LoadChar(character, this);
        }
    }

    public void OpenPack(int packIdx)
    {
        openPackIdx = packIdx;
        LoadChars();
    }

    public void OpenFolder(int idx)
    {
        camMngr.folder.OpenCharacterFolder(idx);
    }

    public void BackButton()
    {
        if (openPackIdx > -1)
            LoadPacks();
    }
}
