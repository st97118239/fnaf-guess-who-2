using System.Collections;
using HeathenEngineering.SteamworksIntegration;
using TMPro;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource vlSource;

    [SerializeField] private TMP_Text subtitleText;
    [SerializeField] private string subtitlePrefix;

    private static AudioManager instance;

    private Coroutine vlCoroutine;

    private Voiceline currentVoiceline;

    private void Awake() => instance = this;

    public static float PlayVoiceline(Voiceline givenVl)
    {
        if (givenVl == null || givenVl.audio == null)
        {
            Debug.LogError("Voiceline is invalid.");
            return -1;
        }

        instance.vlSource.Stop();

        instance.currentVoiceline = givenVl;
        instance.vlSource.clip = givenVl.audio;

        instance.StartVoiceline(!string.IsNullOrEmpty(givenVl.subtitle) ? givenVl.subtitle : string.Empty);

        return givenVl.audio.length;
    }

    private void StartVoiceline(string subtitle)
    {
        if (vlCoroutine != null)
            StopCoroutine(vlCoroutine);
        vlCoroutine = StartCoroutine(VoicelineTimer(subtitle));
    }

    private IEnumerator VoicelineTimer(string subtitle)
    {
        WaitForSeconds timer = new(vlSource.clip.length);

        vlSource.Play();
        if (subtitle != string.Empty)
        {
            subtitleText.text = subtitlePrefix + subtitle;
            subtitleText.gameObject.SetActive(true);
        }
        else
            subtitleText.gameObject.SetActive(false);

        yield return timer;

        if (currentVoiceline.nextVl != null)
        {
            PlayVoiceline(currentVoiceline.nextVl);
        }
        else
            subtitleText.gameObject.SetActive(false);
    }

    public static void StopVoicelines()
    {
        if (instance.vlCoroutine == null)
            return;

        instance.StopVoiceline();
    }

    private void StopVoiceline()
    {
        vlSource.Stop();

        subtitleText.gameObject.SetActive(false);
    }
}
