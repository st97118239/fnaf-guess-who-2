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
        character = CharactersLoader.GetCharacter(packIdx, charIdx);
        charBoardMngr = givenCharBoardMngr;

        Load();
    }

    public void LoadCharBasic(string path)
    {
        character = CharactersLoader.GetCharacter(path);

        Load();
    }

    private void Load()
    {
        Sprite sprite = CharactersLoader.GetCharacterPolaroid(character.path, 0);
        characterImage.sprite = sprite != null ? sprite : CharactersLoader.errorImage;
        characterText.text = character.characterName;
        canvasGroup.alpha = 1;
    }

    public void LoadPack(int givenPackIdx, CharacterBoardManager givenCharBoardMngr)
    {
        packIdx = givenPackIdx;
        pack = CharactersLoader.GetPack(packIdx);

        charBoardMngr = givenCharBoardMngr;
        characterImage.sprite = CharactersLoader.GetPackImage(pack.path);
        characterText.text = pack.packName;
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
    }

    public void Show()
    {
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
