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
    private int idx = -1;
    private CharPack pack;

    private CharacterBoardManager charBoardMngr;
    
    public void LoadChar(Character givenChar, CharacterBoardManager givenCharBoardMngr)
    {
        character = givenChar;

        charBoardMngr = givenCharBoardMngr;
        characterImage.sprite = Resources.Load<Sprite>(character.polaroidsPaths[0]);
        characterText.text = character.characterName;
        canvasGroup.alpha = 1;
    }

    public void LoadPack(int givenPackIdx, CharacterBoardManager givenCharBoardMngr)
    {
        idx = givenPackIdx;
        pack = CharactersLoader.packs[idx];

        charBoardMngr = givenCharBoardMngr;
        characterImage.sprite = Resources.Load<Sprite>(pack.imagePath);
        characterText.text = pack.packName;
        canvasGroup.alpha = 1;
    }

    public void Unload()
    {
        character = null;
        idx = -1;
        pack = null;
        canvasGroup.alpha = 0;
    }

    public void Press()
    {
        if (pack != null)
            charBoardMngr.OpenPack(idx);
        else if (character != null)
            charBoardMngr.OpenFolder(idx);
    }
}
