using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class SettingsContainer : MonoBehaviour
{

    public delegate void Settings (bool b);
    public static Settings OnToggleMusic;

    public static bool EnableMusic = true;
    public static bool EnableHaptic = true;
    public Toggle music, vibrate;

    private CanvasGroup _canvasGroup; 

    void OnEnable ()
    {
        EventManager.OnButtonClick += OnButtonClick;
    }
    void OnDisable ()
    {
        EventManager.OnButtonClick -= OnButtonClick;
    }

    void Start ()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        EnableHaptic = PlayerPrefs.HasKey ("STGS_HAPTIC") ? bool.Parse (PlayerPrefs.GetString ("STGS_HAPTIC")) : true;
        EnableMusic = PlayerPrefs.HasKey ("STGS_MUSIC") ? bool.Parse (PlayerPrefs.GetString ("STGS_MUSIC")) : true;
        music.isOn = EnableMusic;
        vibrate.isOn = EnableHaptic;
        Haptic.Enabled = EnableHaptic;
    }

    public void ToggleMusic ()
    {
        EnableMusic = music.isOn;
        PlayerPrefs.SetString ("STGS_MUSIC", EnableMusic.ToString ());
        OnToggleMusic (EnableMusic);
    }

    public void ToggleHaptic ()
    {
        EnableHaptic = vibrate.isOn;
        Haptic.Enabled = EnableHaptic;
        PlayerPrefs.SetString ("STGS_HAPTIC", Haptic.Enabled.ToString ());
    }

    private void OnButtonClick(ButtonID id, SimpleButtonHandler handler)
    {
        switch(id)
        {
            case ButtonID.B_STGS_OPEN: 
            {
                _canvasGroup.alpha = 1f; 
                _canvasGroup.blocksRaycasts = true; 
                break; 
            }

            case ButtonID.B_STGS_CLOSE: 
            {
                _canvasGroup.alpha = 0f; 
                _canvasGroup.blocksRaycasts = false; 
                break; 
            }
        }
    }
}