using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FolderManager : MonoBehaviour
{
    [SerializeField] private CameraManager camMngr;
    [SerializeField] private CharacterBoardManager charBoardMngr;
    [SerializeField] private ObjMouseHover folderObj;
    [SerializeField] private PaperLine[] paperLines;
    [SerializeField] private RawLine[] linesRaw;

    [SerializeField] private Transform paperLineParent;
    [SerializeField] private PaperLine paperLinePrefab;

    [SerializeField] private GameObject paper;
    [SerializeField] private Image paperImage;

    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private List<int> pages;

    public float totalHeight;
    public int totalPL;
    public int enabledPL;
    public int loadedPL;

    public float maxHeight;

    private int page;
    private bool pageError;

    public Character character { get; private set; }

    private void Awake()
    {
        paperLines = new PaperLine[linesRaw.Length];

        for (int i = 0; i < linesRaw.Length; i++)
        {
            PaperLine pl = Instantiate(paperLinePrefab, paperLineParent);
            paperLines[i] = pl;
            pl.Setup(linesRaw[i], this);
        }
    }

    public void Load()
    {
        if (folderObj.savedIdx == -1)
            return;

        if (character == CharactersLoader.packs[folderObj.savedIdx].characters[folderObj.secondSavedIdx])
            return;

        character = CharactersLoader.packs[folderObj.savedIdx].characters[folderObj.secondSavedIdx];

        page = 0;
        pages = new List<int> { 0 };
        pageError = false;
        totalHeight = 0;
        totalPL = 0;
        enabledPL = 0;
        loadedPL = 0;

        foreach (PaperLine pl in paperLines) 
            pl.Load();

        LoadNewPage(!pageError);
    }

    public void NextPage()
    {
        if (page >= pages.Count - 1) return;
        page++;
        foreach (PaperLine pl in paperLines)
        {
            if (pl.gameObject.activeSelf)
                pl.DisableLine();
        }

        totalHeight = 0;
        enabledPL = 0;

        LoadNewPage(!pageError);
    }

    public void PrevPage()
    {
        if (page <= 0) return;
        page--;
        foreach (PaperLine pl in paperLines)
        {
            if (pl.gameObject.activeSelf)
                pl.DisableLine();
        }

        totalHeight = 0;
        enabledPL = 0;

        LoadNewPage(false);
    }

    private void LoadNewPage(bool shouldAddPages)
    {
        for (int i = pages[page]; i < paperLines.Length; i++)
        {
            if (totalHeight >= maxHeight)
            {
                if (shouldAddPages && pages.Count - 1 == page)
                    pages.Add(i);
                break;
            }

            PaperLine pl = paperLines[i];

            if (pl.canBeEnabled)
                pl.EnableLine();

            if (totalHeight < maxHeight) continue;
            pl.DisableLine();
            if (shouldAddPages && pages.Count - 1 == page)
                pages.Add(i);
            break;
        }

        if (totalHeight == 0)
        {
            Debug.LogError(
                "No info could be displayed on the folder due to no space. Please largen the total height or reduce the amount of text in the string " +
                linesRaw[pages[^1]] + " in " + character.path);
            pages.RemoveRange(pages.Count - 2, 2);
            pageError = true;
            PrevPage();
        }

        if (page == 0 && character.imagesPaths.Length > 0)
        {
            paperImage.sprite = Resources.Load<Sprite>(character.imagesPaths[0]);
            paper.SetActive(true);
        }
        else
        {
            paper.SetActive(false);
        }

        prevButton.interactable = page != 0;
        nextButton.interactable = page != pages.Count - 1;
    }

    public void LoadPackFromLink(string linkedGameID)
    {
        for (int i = 0; i < CharactersLoader.packs.Count; i++)
        {
            if (CharactersLoader.packs[i].linkedGameID != linkedGameID) continue;
            charBoardMngr.OpenPack(i);
            camMngr.characterBoard.Open();
        }
    }
}
