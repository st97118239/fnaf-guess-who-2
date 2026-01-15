using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class CharactersLoader : MonoBehaviour
{
    [SerializeField] private string savePath;

    public static List<CharPack> packs;
    public static List<Character> characters;

    private void Awake()
    {
        LoadCharacters();
    }

    private void SaveCharacters()
    {
#if UNITY_EDITOR
        foreach (Character character in characters)
        {
            string json = JsonUtility.ToJson(character);
            string pathToSaveChar = savePath + character.characterName + ".json";

            if (File.Exists(pathToSaveChar))
            {
                Debug.Log("Replacing file " + pathToSaveChar);
                File.Delete(pathToSaveChar);
            }

            File.WriteAllText(pathToSaveChar, json);
            AssetDatabase.ImportAsset(pathToSaveChar);

            Debug.Log("Saved " + character.characterName);
        }
#endif
        characters.Clear();
    }

    private static void LoadCharacters()
    {
        packs = new();
        characters = new();

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

            string json = packText.text;
            CharPack pack = JsonUtility.FromJson<CharPack>(json);
            pack.characters = new Character[pack.characterPaths.Length];
            pack.path = path;
            pack.imagePath = path + pack.imagePath;
            packs.Add(pack);

            string[] charList = pack.characterPaths;

            for (int i = 0; i < charList.Length; i++)
            {
                string charDir = charList[i];
                if (!File.Exists("Assets/Resources/Packs/" + packDir + "/" + charDir + "/Char.json"))
                {
                    Debug.LogError("Character " + charDir + " in the pack " + packDir + " could not be found. Please check if your spelling is correct.");
                    continue;
                }

                path = "Packs/" + packDir + "/" + charDir + "/";
                TextAsset charText = Resources.Load<TextAsset>(path + "Char");

                json = charText.text;
                Character character = JsonUtility.FromJson<Character>(json);

                character.path = path;

                for (int j = 0; j < character.polaroidsPaths.Length; j++)
                {
                    string imgPath = character.polaroidsPaths[j];
                    character.polaroidsPaths[j] = path + imgPath;
                }

                for (int j = 0; j < character.imagesPaths.Length; j++)
                {
                    string imgPath = character.imagesPaths[j];
                    character.imagesPaths[j] = path + imgPath;
                }

                Debug.Log("Found character " + character.characterName + " from pack " + packDir);
                pack.characters[i] = character;
                characters.Add(character);
            }
        }
    }
}
