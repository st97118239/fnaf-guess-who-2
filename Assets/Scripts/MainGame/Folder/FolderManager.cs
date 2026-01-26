using System.Collections;
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

    [SerializeField] private Paper paperPrefab;
    [SerializeField] private Transform paperParent;
    [SerializeField] private Paper[] imagePapers;
    [SerializeField] private int enabledImgPapers;
    [SerializeField] private int imgPapersIdx;
    [SerializeField] private int maxImgPapers;

    [SerializeField] private Polaroid polaroid;

    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button audioButton;

    [SerializeField] private List<int> pages;

    [SerializeField] private int voicelineChance;

    public float totalHeight;
    public int totalPL;
    public int enabledPL;
    public int loadedPL;

    public float maxHeight;
    public float polaroidHeight;
    public float maxWidthWhenPolaroid;

    public int page;
    private bool pageError;

    private bool canMoveImgPapers;
    private bool isMovingBack;
    private bool isMovingPaper;

    private Coroutine vlCoroutine;

    public Character character { get; private set; }

    private void Awake()
    {
        if (voicelineChance > 100)
        {
            voicelineChance = 100;
            Debug.LogWarning("VoicelineChance should not be over 100%!");
        }

        paperLines = new PaperLine[linesRaw.Length];

        for (int i = 0; i < linesRaw.Length; i++)
        {
            PaperLine pl = Instantiate(paperLinePrefab, paperLineParent);
            paperLines[i] = pl;
            pl.Setup(linesRaw[i], this);
        }

        GeneratePaperImages(maxImgPapers);
    }

    public void Load()
    {
        if (folderObj.savedIdx == -1)
            return;

        Character foundChar = CharactersLoader.GetCharacter(folderObj.savedIdx, folderObj.secondSavedIdx);

        if (character == foundChar)
        {
            CheckIfVl();

            return;
        }

        StopVoiceline();

        character = foundChar;

        CharactersLoader.LoadVoicelines(character.path);
        CheckIfVl();

        polaroid.LoadCharBasic(character.path);

        page = 0;
        pages = new List<int> { 0 };
        pageError = false;
        totalHeight = 0;
        totalPL = 0;
        enabledPL = 0;
        loadedPL = 0;
        canMoveImgPapers = false;
        isMovingBack = false;
        isMovingPaper = false;
        enabledImgPapers = 0;
        imgPapersIdx = 0;


        if (character.voicelines != null && character.voicelines.Length > 0)
            audioButton.interactable = true;
        else
            audioButton.interactable = false;

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

        foreach (Paper paper in imagePapers)
        {
            paper.isOn = false;
        }

        if (page == 0)
        {
            polaroid.Show();

            if (character.imagesPaths.Length > 0)
            {
                if (imagePapers.Length < character.imagesPaths.Length) 
                    GeneratePaperImages(character.imagesPaths.Length);

                for (int i = 0; i < character.imagesPaths.Length; i++)
                {
                    imagePapers[i].Load(CharactersLoader.GetCharacterImage(character.path, i));
                }

                enabledImgPapers = character.imagesPaths.Length;
                canMoveImgPapers = enabledImgPapers > 1;
            }
        }
        else
            polaroid.Hide();

        foreach (Paper paper in imagePapers)
        {
            if (!paper.isOn)
                paper.Unload();

            StartCoroutine(paper.ResetPos());
        }

        isMovingPaper = false;
        isMovingBack = false;
        imgPapersIdx = 0;
        prevButton.interactable = page != 0;
        nextButton.interactable = page != pages.Count - 1;
    }

    public void LoadPackFromLink(string linkedGameID)
    {
        for (int i = 0; i < CharactersLoader.packs.Count; i++)
        {
            if (CharactersLoader.GetPack(i).linkedGameID != linkedGameID) continue;
            charBoardMngr.OpenPack(i);
            camMngr.characterBoard.Open();
        }
    }

    private void GeneratePaperImages(int max)
    {
        foreach (Paper paper in imagePapers)
        {
            Destroy(paper.gameObject);
        }

        imagePapers = new Paper[max];
        maxImgPapers = max;

        for (int i = 0; i < max; i++)
        {
            Paper paper = Instantiate(paperPrefab, paperParent);
            paper.Setup(this, i);
            imagePapers[i] = paper;
        }
    }

    public void PaperMoveNext()
    {
        if (!canMoveImgPapers || imgPapersIdx >= enabledImgPapers - 1) return;
        canMoveImgPapers = false;
        isMovingBack = false;

        imagePapers[imgPapersIdx].MoveAnim(false);

        for (int i = 0; i < imgPapersIdx; i++)
        {
            imagePapers[i].Move(true);
        }

        imgPapersIdx++;
    }

    public void PaperMoveBack()
    {
        if (!canMoveImgPapers || imgPapersIdx <= 0) return;
        canMoveImgPapers = false;
        isMovingBack = true;

        imgPapersIdx--;
        imagePapers[imgPapersIdx].MoveAnim(true);

        for (int i = imgPapersIdx + 1; i < enabledImgPapers; i++)
        {
            imagePapers[i].Move(true);
        }
    }

    public void FinishedMovingPapers()
    {
        if (!isMovingPaper)
        {
            isMovingPaper = true;
            return;
        }

        if (isMovingBack)
        {
            for (int i = 0; i < imgPapersIdx; i++)
            {
                imagePapers[i].Move(false);
            }
        }
        else
        {
            for (int i = imgPapersIdx; i < enabledImgPapers; i++)
            {
                imagePapers[i].Move(false);
            }
        }

        isMovingPaper = false;
        canMoveImgPapers = true;
    }

    public void AudioButton()
    {
        if (vlCoroutine != null)
            StopCoroutine(vlCoroutine);
        vlCoroutine = StartCoroutine(AudioTimer(AudioManager.PlayVoiceline(CharactersLoader.GetCharacterRngVoiceline(character.path))));
    }

    private IEnumerator AudioTimer(float time)
    {
        audioButton.interactable = false;

        yield return new WaitForSeconds(time);

        audioButton.interactable = true;
    }

    private void StopVoiceline()
    {
        if (vlCoroutine == null) return;

        StopCoroutine(vlCoroutine);
        AudioManager.StopVoicelines();
    }

    private void CheckIfVl()
    {
        if (Random.Range(1, 100) <= voicelineChance) 
            AudioButton();
    }
}
