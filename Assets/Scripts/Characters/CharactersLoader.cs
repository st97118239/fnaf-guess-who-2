using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Overlays;

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

        int idx = 0;

        while (true)
        {
            TextAsset charText = Resources.Load<TextAsset>(path + idx);

            if (charText == null)
            {
                Debug.Log("No more characters found.");
                break;
            }

            string json = charText.text;

            Character character = JsonUtility.FromJson<Character>(json);

            Debug.Log("Found character " + character.characterName);
            characters.Add(character);
            idx++;
        }
    }
}
