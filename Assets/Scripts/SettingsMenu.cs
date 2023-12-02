using UnityEngine.Audio;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mainMixer;
    public GameObject canvas;
    public GameObject AreYouSure;

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
    }

    public void ExitButton()
    {
        canvas.SetActive(!canvas.activeSelf);
    }

    public void ResetGameData()
    {
        AreYouSure.SetActive(!AreYouSure.activeSelf);
    }
}
