using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PaperLine : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform rectTrans;
    [SerializeField] private TMP_Text line;
    [SerializeField] private RectTransform lineRectTrans;
    [SerializeField] private TMP_Text answer;
    [SerializeField] private RectTransform answerRectTrans;

    [SerializeField] private float spaceBeforeAnswer;

    public bool canBeEnabled;

    private FolderManager fldrMngr;

    private RawLine rawLine;

    private bool isString;
    private bool isArray;
    private bool isInt;
    private bool isFloat;

    private Camera cam;

    public void Setup(RawLine givenRawLine, FolderManager givenFolderManager)
    {
        cam = Camera.main;

        fldrMngr = givenFolderManager;
        rawLine = givenRawLine;

        isString = false;
        isArray = false;
        isInt = false;
        isFloat = false;

        switch (rawLine)
        {
            case RawLine.characterName:
                isString = true;
                line.text = "Name:";
                break;
            case RawLine.pronouns:
                isString = true;
                line.text = "Pronouns:";
                break;
            case RawLine.description:
                isString = true;
                line.text = "Description:";
                break;
            case RawLine.decadeMade:
                isInt = true;
                line.text = "Decade made:";
                break;
            case RawLine.yearMade:
                isInt = true;
                line.text = "Year made:";
                break;
            case RawLine.yearDied:
                isInt = true;
                line.text = "Year died:";
                break;
            case RawLine.reasonOfDeath:
                isString = true;
                line.text = "Reason of death:";
                break;
            case RawLine.familyMembers:
                isString = true;
                isArray = true;
                line.text = "Family members:";
                break;
            case RawLine.owners:
                isString = true;
                isArray = true;
                line.text = "Owner:";
                break;
            case RawLine.locations:
                isString = true;
                isArray = true;
                line.text = "Locations:";
                break;
            case RawLine.occupation:
                isString = true;
                line.text = "Occupation:";
                break;
            case RawLine.employment:
                isString = true;
                isArray = true;
                line.text = "Employment:";
                break;
            case RawLine.affiliation:
                isString = true;
                isArray = true;
                line.text = "Affiliation:";
                break;
            case RawLine.aliases:
                isString = true;
                isArray = true;
                line.text = "Aliases:";
                break;
            case RawLine.animClass:
                isString = true;
                line.text = "Class:";
                break;
            case RawLine.model:
                isString = true;
                line.text = "Model:";
                break;
            case RawLine.height:
                isFloat = true;
                line.text = "Height:";
                break;
            case RawLine.weight:
                isFloat = true;
                line.text = "Weight:";
                break;
            case RawLine.mainColor:
                isString = true;
                line.text = "Primary colour:";
                break;
            case RawLine.secondaryColor:
                isString = true;
                line.text = "Secondary colour:";
                break;
            case RawLine.eyeColor:
                isString = true;
                line.text = "Eye colour:";
                break;
            case RawLine.clothes:
                isString = true;
                isArray = true;
                line.text = "Clothing:";
                break;
            case RawLine.teethType:
                isString = true;
                line.text = "Teeth:";
                break;
            case RawLine.armCount:
                isInt = true;
                line.text = "Arms:";
                break;
            case RawLine.legCount:
                isInt = true;
                line.text = "Legs:";
                break;
            case RawLine.state:
                isString = true;
                line.text = "State:";
                break;
            case RawLine.hasKilled:
                isString = true;
                isArray = true;
                line.text = "Has killed:";
                break;
            case RawLine.firstAppearance:
                isString = true;
                line.text = "First appearance:";
                break;
            case RawLine.majorAppearances:
                isString = true;
                isArray = true;
                line.text = "Major appearances:";
                break;
            case RawLine.minorAppearances:
                isString = true;
                isArray = true;
                line.text = "Minor appearances:";
                break;
            case RawLine.cameos:
                isString = true;
                isArray = true;
                line.text = "Cameos:";
                break;
            case RawLine.voiceActor:
                isString = true;
                line.text = "Voice actor:";
                break;
        }

        gameObject.SetActive(false);
    }

    public void Load()
    {
        gameObject.SetActive(false);

        if (isArray)
        {
            if (isString) 
                LoadStringArray();
        }
        else
        {
            if (isString)
                LoadString();
            else if (isInt)
                LoadInt();
            else if (isFloat)
                LoadFloat();
        }

        if (answer.text == string.Empty)
        {
            canBeEnabled = false;
            return;
        }

        canBeEnabled = true;
        fldrMngr.totalPL++;
    }

    public void EnableLine()
    {
        gameObject.SetActive(true);
        fldrMngr.enabledPL++;
        lineRectTrans.sizeDelta = new Vector2(line.preferredWidth + spaceBeforeAnswer, line.preferredHeight);
        answerRectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x - lineRectTrans.sizeDelta.x, 27.93f);
        answerRectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x - lineRectTrans.sizeDelta.x, answer.preferredHeight);
        lineRectTrans.sizeDelta = new Vector2(line.preferredWidth + spaceBeforeAnswer, answer.preferredHeight);
        rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, answer.preferredHeight);
        fldrMngr.totalHeight += rectTrans.sizeDelta.y;
    }

    public void DisableLine()
    {
        gameObject.SetActive(false);
        fldrMngr.enabledPL--;
        fldrMngr.totalHeight -= rectTrans.sizeDelta.y;
    }

    private void LoadString()
    {
        string foundAnswer = rawLine switch
        {
            RawLine.characterName => fldrMngr.character.characterName,
            RawLine.pronouns => fldrMngr.character.pronouns,
            RawLine.description => fldrMngr.character.description,
            RawLine.reasonOfDeath => fldrMngr.character.reasonOfDeath,
            RawLine.occupation => fldrMngr.character.occupation,
            RawLine.animClass => fldrMngr.character.animClass,
            RawLine.model => fldrMngr.character.model,
            RawLine.mainColor => fldrMngr.character.mainColor,
            RawLine.secondaryColor => fldrMngr.character.secondaryColor,
            RawLine.eyeColor => fldrMngr.character.eyeColor,
            RawLine.teethType => fldrMngr.character.teethType,
            RawLine.state => fldrMngr.character.state,
            RawLine.firstAppearance => fldrMngr.character.firstAppearance,
            RawLine.voiceActor => fldrMngr.character.voiceActor,
            _ => null
        };

        if (foundAnswer == string.Empty)
        {
            gameObject.SetActive(false);
            answer.text = string.Empty;
            return;
        }

        if (rawLine == RawLine.firstAppearance)
        {
            string text = foundAnswer;
            string word = KeywordToGame(text);

            foundAnswer = "<#0000EE><u><link=\"" + text + "\">" + word + "</link></u></color>";
        }

        answer.text = foundAnswer;
    }

    private void LoadStringArray()
    {
        string[] foundAnswer;
        switch (rawLine)
        {
            case RawLine.familyMembers:
                foundAnswer = fldrMngr.character.familyMembers;
                break;
            case RawLine.owners:
                foundAnswer = fldrMngr.character.owners;
                line.text = foundAnswer.Length > 1 ? "Owners:" : "Owner:";
                break;
            case RawLine.locations:
                foundAnswer = fldrMngr.character.locations;
                line.text = foundAnswer.Length > 1 ? "Locations:" : "Location:";
                break;
            case RawLine.employment:
                foundAnswer = fldrMngr.character.employment;
                break;
            case RawLine.affiliation:
                foundAnswer = fldrMngr.character.affiliation;
                break;
            case RawLine.aliases:
                foundAnswer = fldrMngr.character.aliases;
                line.text = foundAnswer.Length > 1 ? "Aliases:" : "Alias:";
                break;
            case RawLine.clothes:
                foundAnswer = fldrMngr.character.clothes;
                break;
            case RawLine.hasKilled:
                foundAnswer = fldrMngr.character.hasKilled;
                break;
            case RawLine.majorAppearances:
                foundAnswer = fldrMngr.character.majorAppearances;
                line.text = foundAnswer.Length > 1 ? "Major appearances:" : "Major appearance:";
                break;
            case RawLine.minorAppearances:
                foundAnswer = fldrMngr.character.minorAppearances;
                line.text = foundAnswer.Length > 1 ? "Minor appearances:" : "Minor appearance:";
                break;
            case RawLine.cameos:
                foundAnswer = fldrMngr.character.cameos;
                line.text = foundAnswer.Length > 1 ? "Cameos:" : "Cameo:";
                break;
            default:
                foundAnswer = null;
                break;
        }

        if (foundAnswer == null || foundAnswer.Length == 0)
        {
            gameObject.SetActive(false);
            answer.text = string.Empty;
            return;
        }

        string[] answers = new string[foundAnswer.Length];

        for (int i = 0; i < foundAnswer.Length; i++)
        {
            answers[i] = foundAnswer[i];
        }

        if (rawLine is RawLine.majorAppearances or RawLine.minorAppearances or RawLine.cameos)
        {
            for (int i = 0; i < answers.Length; i++)
            {
                string text = answers[i];
                string word = KeywordToGame(text);

                answers[i] = "<#0000EE><u><link=\"" + text + "\">" + word + "</link></u></color>";
            }
        }

        string fullText = string.Empty;

        for (int i = 0; i < answers.Length; i++)
        {
            if (i > 0)
                fullText += ", ";
            fullText += answers[i];
        }

        answer.text = fullText;
    }

    private void LoadInt()
    {
        int foundAnswer;
        string extraText = string.Empty;
        switch (rawLine)
        {
            case RawLine.decadeMade:
                foundAnswer = fldrMngr.character.decadeMade;
                extraText = "'s";
                line.text = "Decade made:";
                if (fldrMngr.character.estimatedYear)
                    line.text += " (estimated)";
                break;
            case RawLine.yearMade:
                foundAnswer = fldrMngr.character.yearMade;
                line.text = fldrMngr.character.isBorn ? "Year made:" : "Year born:";
                if (fldrMngr.character.estimatedYear)
                    line.text += " (estimated)";
                break;
            case RawLine.yearDied:
                foundAnswer = fldrMngr.character.yearDied;
                break;
            case RawLine.armCount:
                foundAnswer = fldrMngr.character.armCount;
                break;
            case RawLine.legCount:
                foundAnswer = fldrMngr.character.legCount;
                break;
            default:
                foundAnswer = -1;
                break;
        }

        if (foundAnswer == -1)
        {
            gameObject.SetActive(false);
            answer.text = string.Empty;
            return;
        }

        answer.text = foundAnswer + extraText;
    }

    private void LoadFloat()
    {
        float foundAnswer;
        string extraText = string.Empty;
        switch (rawLine)
        {
            case RawLine.height:
                foundAnswer = fldrMngr.character.height;
                extraText = " cm";
                break;
            case RawLine.weight:
                foundAnswer = fldrMngr.character.weight;
                extraText = " kg";
                break;
            default:
                foundAnswer = -1;
                break;
        }

        if (foundAnswer < 0)
        {
            gameObject.SetActive(false);
            answer.text = string.Empty;
            return;
        }

        answer.text = foundAnswer + extraText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Vector3 mousePos = new(eventData.position.x, eventData.position.y, 0);

        int linkTaggedText = TMP_TextUtilities.FindIntersectingLink(answer, mousePos, cam);

        if (linkTaggedText == -1) return;
        TMP_LinkInfo linkInfo = answer.textInfo.linkInfo[linkTaggedText];

        OnTextClicked(linkInfo.GetLinkText());
    }

    private void OnTextClicked(string link)
    {
        fldrMngr.LoadPackFromLink(GameToKeyword(link));
    }

    public static string KeywordToGame(string keyword)
    {
        string game = keyword switch
        {
            "fnaf1" => "Five Nights at Freddy's",
            "fnaf2" => "Five Nights at Freddy's 2",
            "fnaf3" => "Five Nights at Freddy's 3",
            "fnaf4" => "Five Nights at Freddy's 4",
            "fnafWorld" => "FNaF World",
            "fnafSL" => "Five Nights at Freddy's: Sister Location",
            "pizzaSim" => "Freddy Fazbear's Pizzeria Simulator",
            "ucn" => "Ultimate Custom Night",
            "fnafHW" => "Five Nights at Freddy's: Help Wanted",
            "fnafSD" => "Five Nights at Freddy's AR: Special Delivery",
            "fnafSB" => "Five Nights at Freddy's: Security Breach",
            "fnafSBR" => "Five Nights at Freddy's: Security Breach RUIN",
            "fnafHW2" => "Five Nights at Freddy's: Help Wanted 2",
            "fnafItP" => "Five Nights at Freddy's: Into the Pit",
            "fnafSotM" => "Five Nights at Freddy's: Secret of the Mimic",
            _ => keyword
        };

        return game;
    }

    public static string GameToKeyword(string keyword)
    {
        string game = keyword switch
        {
            "Five Nights at Freddy's" => "fnaf1",
            "Five Nights at Freddy's 2" => "fnaf2",
            "Five Nights at Freddy's 3" => "fnaf3",
            "Five Nights at Freddy's 4" => "fnaf4",
            "FNaF World" => "fnafWorld",
            "Five Nights at Freddy's: Sister Location" => "fnafSL",
            "Freddy Fazbear's Pizzeria Simulator" => "pizzaSim",
            "Ultimate Custom Night" => "ucn",
            "Five Nights at Freddy's: Help Wanted" => "fnafHW",
            "Five Nights at Freddy's AR: Special Delivery" => "fnafSD",
            "Five Nights at Freddy's: Security Breach" => "fnafSB",
            "Five Nights at Freddy's: Security Breach RUIN" => "fnafSBR",
            "Five Nights at Freddy's: Help Wanted 2" => "fnafHW2",
            "Five Nights at Freddy's: Into the Pit" => "fnafItP",
            "Five Nights at Freddy's: Secret of the Mimic" => "fnafSotM",
            _ => keyword
        };

        return game;
    }
}
