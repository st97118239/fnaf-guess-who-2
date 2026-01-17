using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private Image mainImage;
    [SerializeField] private Image xImage;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup canvasGroup;

    private CharacterBoardManager charBoardMngr;
    
    public void Setup(CharacterBoardManager givenCharBoardMngr)
    {
        charBoardMngr = givenCharBoardMngr;

    }

    public void SetText(string givenText)
    {
        text.text = givenText;
    }
    public void Load()
    {
        canvasGroup.alpha = 1;
    }
    public void Unload()
    {
        canvasGroup.alpha = 0;
    }

    public void Press()
    {

    }
}
