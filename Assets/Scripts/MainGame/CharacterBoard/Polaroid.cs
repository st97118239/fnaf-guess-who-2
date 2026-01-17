using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Polaroid : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private TMP_Text characterText;
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup canvasGroup;

    private Character character;
    private int packIdx = -1;
    private int charIdx = -1;
    private CharPack pack;

    private CharacterBoardManager charBoardMngr;
    
    public void LoadChar(int givenPackIdx, int givenCharIdx, CharacterBoardManager givenCharBoardMngr)
    {
        packIdx = givenPackIdx;
        charIdx = givenCharIdx;
        character = CharactersLoader.packs[packIdx].characters[charIdx];

        charBoardMngr = givenCharBoardMngr;
        characterImage.sprite = Resources.Load<Sprite>(character.polaroidsPaths[0]);
        characterText.text = character.characterName;
        canvasGroup.alpha = 1;
    }

    public void LoadPack(int givenPackIdx, CharacterBoardManager givenCharBoardMngr)
    {
        packIdx = givenPackIdx;
        pack = CharactersLoader.packs[packIdx];

        charBoardMngr = givenCharBoardMngr;
        characterImage.sprite = Resources.Load<Sprite>(pack.imagePath);
        characterText.text = pack.packName;
        canvasGroup.alpha = 1;
    }

    public void Unload()
    {
        character = null;
        packIdx = -1;
        charIdx = -1;
        pack = null;
        canvasGroup.alpha = 0;
    }

    public void Press()
    {
        if (pack != null)
            charBoardMngr.OpenPack(packIdx);
        else if (character != null)
            charBoardMngr.OpenFolder(packIdx, charIdx);
    }
}
