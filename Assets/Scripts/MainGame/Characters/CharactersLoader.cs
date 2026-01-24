using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharactersLoader : MonoBehaviour
{
    [SerializeField] private string savePath;

    public static List<string> packs;
    public static List<string> characters;

    public static Dictionary<string, CharPack> packDict;
    public static Dictionary<string, Character> charDict;

    public List<string> _packs;
    public List<string> _characters;

    public List<CharPack> _packsList;
    public List<Character> _charactersList;

    private static Sprite errorImage;

    [SerializeField] private Sprite _errorImage;

    private void Awake()
    {
        errorImage = _errorImage;
        LoadCharacters();

#if UNITY_EDITOR
        _packs = packs;
        _characters = characters;

        foreach (string packPath in _packs)
        {
            _packsList.Add(packDict[packPath]);
        }

        foreach (string charPath in _characters)
        {
            _charactersList.Add(charDict[charPath]);
        }
#endif
    }

    private static void LoadCharacters()
    {
        packs = new();
        characters = new();
        packDict = new();
        charDict = new();

        string[] packList = File.ReadAllLines("Assets/Resources/Packs/Packs.txt");

        foreach (string packDir in packList)
        {
            if (!File.Exists("Assets/Resources/Packs/" + packDir + "/Pack.json"))
            {
                Debug.LogError("Pack " + packDir + " could not be found. Please check if your spelling is correct.");
                continue;
            }

            string path = "Packs/" + packDir + "/";

            TextAsset packText = Resources.Load<TextAsset>(path + "Pack");

            if (packText.text == string.Empty)
            {
                Debug.LogError("Pack.json at " + packDir + " is empty.");
                continue;
            }

            string json = packText.text;
            CharPack pack;
            try
            {
                pack = JsonUtility.FromJson<CharPack>(json);
            }
            catch
            {
                Debug.LogError("Pack.json at " + packDir + " is invalid.");
                continue;
            }

            if (string.IsNullOrEmpty(pack.packName))
            {
                Debug.LogError("The pack name in " + packDir + " is invalid.");
                continue;
            }

            pack.path = path;

            if (pack.imagePath != string.Empty)
            {
                if (File.Exists("Assets/Resources/" + path + pack.imagePath + ".png"))
                {
                    pack.imagePath = path + pack.imagePath;
                }
                else
                {
                    pack.imagePath = string.Empty;
                    Debug.LogWarning("Pack " + pack.packName + " has no image.");
                }
            }
            else
                Debug.LogWarning("Pack " + pack.packName + " has no image.");

            string[] charList = File.ReadAllLines("Assets/Resources/" + path + "Characters.txt");

            if (charList.Length == 0)
            {
                Debug.LogError("Character list for " + pack.packName + " is empty.");
                continue;
            }

            Debug.Log("Found pack " + pack.packName + ". Loading characters...");

            packs.Add(path);
            packDict.Add(path, pack);
            pack.characterPaths = new string[charList.Length];

            bool hasError = false;

            for (int i = 0; i < charList.Length; i++)
            {
                string charDir = charList[i];
                if (!File.Exists("Assets/Resources/Packs/" + packDir + "/" + charDir + "/Char.json"))
                {
                    Debug.LogError("Character " + charDir + " in the pack " + packDir + " could not be found.");
                    pack.characterPaths[i] = null;
                    hasError = true;
                    continue;
                }

                path = "Packs/" + packDir + "/" + charDir + "/";
                TextAsset charText = Resources.Load<TextAsset>(path + "Char");

                if (charText.text == string.Empty)
                {
                    Debug.LogError("Char.json for " + charDir + " is empty.");
                    pack.characterPaths[i] = null;
                    hasError = true;
                    continue;
                }

                json = charText.text;

                Character character;
                try
                {
                    character = JsonUtility.FromJson<Character>(json);
                }
                catch
                {
                    Debug.LogError("Char.json for character " + charDir + " is invalid.");
                    pack.characterPaths[i] = null;
                    hasError = true;
                    continue;
                }

                if (string.IsNullOrEmpty(character.characterName))
                {
                    Debug.LogError("The character name for " + charDir + " is invalid.");
                    pack.characterPaths[i] = null;
                    hasError = true;
                    continue;
                }

                Debug.Log("Found character " + character.characterName + " from pack " + packDir);

                character.path = path;
                if (character.polaroidsPaths.Length > 0)
                {
                    for (int j = 0; j < character.polaroidsPaths.Length; j++)
                    {
                        string imgPath = character.polaroidsPaths[j];
                        character.polaroidsPaths[j] = path + imgPath;
                    }
                }
                else
                {
                    Debug.LogWarning("Character " + character.characterName + " has no polaroid images.");
                }

                if (character.imagesPaths.Length > 0)
                {
                    for (int j = 0; j < character.imagesPaths.Length; j++)
                    {
                        string imgPath = character.imagesPaths[j];
                        character.imagesPaths[j] = path + imgPath;
                    }
                }
                else
                {
                    Debug.LogWarning("Character " + character.characterName + " has no full images.");
                }
                
                characters.Add(path);
                pack.characterPaths[i] = path;
                charDict.Add(path, character);
            }

            if (hasError)
            {
                string[] tempPaths = pack.characterPaths;

                for (int i = 0; i < tempPaths.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tempPaths[i])) continue;

                    for (int a = i; a < pack.characterPaths.Length - 1; a++)
                    {
                        pack.characterPaths[a] = pack.characterPaths[a + 1];
                    }
                    Array.Resize(ref pack.characterPaths, pack.characterPaths.Length - 1);
                }
            }

            Debug.Log("Loaded all characters from pack " + pack.packName + ".");
        }
    }

    private static void LoadPackImage(string packPath)
    {
        CharPack pack = packDict[packPath];

        if (pack.imagePath == string.Empty)
        {
            pack.image = errorImage;
            Debug.LogWarning("There is no image for " + pack.packName);
            return;
        }

        Sprite img;

        try
        {
            img = Resources.Load<Sprite>(pack.imagePath);
        }
        catch
        {
            img = errorImage;
            Debug.LogError("Image " + pack.imagePath + " is invalid. Make sure the image is an actual image and that it's a png.");
        }

        if (img == null)
        {
            img = errorImage;
            Debug.LogError("Image " + pack.imagePath + " is invalid. Make sure the image is an actual image and that it's a png.");
        }

        pack.image = img;
    }

    private static void LoadCharPolaroids(string charPath)
    {
        Character character = charDict[charPath];

        if (character.polaroidsPaths.Length == 0)
        {
            character.polaroids = new []{ errorImage };
            Debug.LogWarning("There are no polaroids for " + character.characterName);
            return;
        }

        character.polaroids = new Sprite[character.polaroidsPaths.Length];

        for (int i = 0; i < character.polaroidsPaths.Length; i++)
        {
            Sprite img;
            try
            {
                img = Resources.Load<Sprite>(character.polaroidsPaths[i]);
            }
            catch
            {
                img = errorImage;
                Debug.LogError("Polaroid " + character.polaroidsPaths[i] + " is invalid. Make sure the image is an actual image and that it's a png.");
            }

            if (img == null)
            {
                img = errorImage;
                Debug.LogError("Polaroid " + character.polaroidsPaths[i] + " is invalid. Make sure the image is an actual image and that it's a png.");
            }

            character.polaroids[i] = img;
        }
    }

    private static void LoadCharImages(string charPath)
    {
        Character character = charDict[charPath];

        if (character.imagesPaths.Length == 0)
        {
            character.images = new []{ errorImage };
            Debug.LogWarning("There are no images for " + character.characterName);
            return;
        }

        character.images = new Sprite[character.imagesPaths.Length];

        for (int i = 0; i < character.imagesPaths.Length; i++)
        {
            Sprite img;
            try
            {
                img = Resources.Load<Sprite>(character.imagesPaths[i]);
            }
            catch
            {
                img = errorImage;
                Debug.LogError("Image " + character.imagesPaths[i] + " is invalid. Make sure the image is an actual image and that it's a png.");
            }

            if (img == null)
            {
                img = errorImage;
                Debug.LogError("Image " + character.imagesPaths[i] + " is invalid. Make sure the image is an actual image and that it's a png.");
            }

            character.images[i] = img;
        }
    }

    public static CharPack GetPack(int idx)
    {
        return packDict[packs[idx]];
    }

    public static string[] GetCharacterPathsFromPack(int idx)
    {
        return packDict[packs[idx]].characterPaths;
    }

    public static string[] GetCharactersPathsFromPack(string path)
    {
        return packDict[path].characterPaths;
    }

    public static Character GetCharacter(string path)
    {
        return charDict[path];
    }

    public static Character GetCharacter(int idx)
    {
        return charDict[characters[idx]];
    }

    public static string GetCharacterPath(int packIdx, int charIdx)
    {
        return GetCharacterPathsFromPack(packIdx)[charIdx];
    }

    public static Character GetCharacter(int packIdx, int charIdx)
    {
        return GetCharacter(GetCharacterPathsFromPack(packIdx)[charIdx]);
    }

    public static Sprite GetPackImage(string packPath)
    {
        CharPack pack = packDict[packPath];

        if (pack.image == null)
        {
            LoadPackImage(packPath);
        }

        if (pack.image != null) return pack.image;

        Debug.LogError("Couldn't find the image for pack " + pack.packName);
        return null;

    }

    public static Sprite GetCharacterPolaroid(string charPath, int idx)
    {
        Character character = charDict[charPath];

        if (character.polaroids == null || character.polaroids.Length == 0)
        {
            LoadCharPolaroids(charPath);
        }

        if (character.polaroids == null || character.polaroids.Length == 0)
        {
            Debug.LogError("No polaroids found for character " + character.characterName);
            return null;
        }

        if (character.polaroids.Length >= idx + 1) return character.polaroids[idx];

        Debug.LogError("Not enough polaroids for character " + character.characterName + ". Given index is " + idx);
        return null;
    }

    public static Sprite GetCharacterImage(string charPath, int idx)
    {
        Character character = charDict[charPath];

        if (character.images == null || character.images.Length == 0)
        {
            LoadCharImages(charPath);
        }

        if (character.images == null || character.images.Length == 0)
        {
            Debug.LogError("No images found for character " + character.characterName);
            return null;
        }

        if (character.images.Length >= idx + 1) return character.images[idx];

        Debug.LogError("Not enough images for character " + character.characterName + ". Given index is " + idx);
        return null;
    }
}
