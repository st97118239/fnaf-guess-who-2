using UnityEngine;

[System.Serializable]
public class Voiceline
{
    public string audioPath;
    public AudioClip audio;
    public string subtitle;
    public string nextVlPath;
    public Voiceline nextVl;
}
