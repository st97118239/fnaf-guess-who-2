using UnityEngine;

[System.Serializable]
public class Character
{
    public string characterName;
    public string shortCharacterName;
    public string pronouns;
    public string description;
    public int decadeMade = -1;
    public int yearMade = -1;
    public bool estimatedYear;
    public bool isBorn;
    public int yearDied = -1;
    public string reasonOfDeath;
    public string[] familyMembers;
    public string[] owners;
    public string[] locations;
    public string occupation;
    public string[] employment;
    public string[] affiliation;
    public string[] aliases;
    public string animClass;
    public string model;
    public float height = -1;
    public float weight = -1;
    public string mainColor;
    public string secondaryColor;
    public string eyeColor;
    public string[] clothes;
    public string teethType;
    public int armCount = -1;
    public int legCount = -1;
    public string state;
    public string[] hasKilled;
    public string firstAppearance;
    public string[] majorAppearances;
    public string[] minorAppearances;
    public string[] cameos;
    public string voiceActor;
    public string[] polaroidsPaths;
    public Sprite[] polaroids;
    public string[] imagesPaths;
    public Sprite[] images;
    public Voiceline[] voicelines;
    public string path;
}
