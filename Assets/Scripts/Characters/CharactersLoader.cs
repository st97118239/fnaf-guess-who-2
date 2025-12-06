using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif

public class CharactersLoader : MonoBehaviour
{
    public string path;
    public string savePath;
    public int charCount;

    public List<Character> characters;

    private void Start()
    {
        if (characters.Count > 0) 
            SaveCharacters();

        LoadCharacters();
    }

    private void SaveCharacters()
    {
#if UNITY_EDITOR
        foreach (Character character in characters)
        {
            string json = JsonUtility.ToJson(character);
            string pathToSaveChar = savePath + character.idx + ".json";

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

    private void LoadCharacters()
    {
        characters = new();

        for (int i = 0; i < charCount; i++)
        {
            TextAsset charText = Resources.Load<TextAsset>(path + i);

            if (charText == null)
            {
                Debug.LogWarning("No character found at " + i + ". Trying on next index, if there's nothing else then please lower the character count.");
                continue;
            }

            string json = charText.text;
            Character character = JsonUtility.FromJson<Character>(json);

            Debug.Log("Found character " + character.characterName);
            characters.Add(character);
        }
    }
}
