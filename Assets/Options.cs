using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public Toggle musicToggle;

    public Toggle sfxToggle;
    // Start is called before the first frame update
    void Awake()
    {
        SetPreviousToggles();
    }

    private void SetPreviousToggles()
    {
        if (PlayerPrefs.GetString("sfx") != String.Empty)
        {
            sfxToggle.isOn = bool.Parse(PlayerPrefs.GetString("sfx"));
        }
        if (PlayerPrefs.GetString("music") != String.Empty)
        {
            musicToggle.isOn = bool.Parse(PlayerPrefs.GetString("music"));
        }
    }

    public void SetMusicToggle()
    {
        bool musicOn = musicToggle.isOn;
        if (musicOn)
        {
            SceneController.Instance.gameObject.GetComponent<AudioSource>().enabled = true;
            if (!SceneController.Instance.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                SceneController.Instance.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else
        {
            SceneController.Instance.gameObject.GetComponent<AudioSource>().enabled = false;
            SceneController.Instance.gameObject.GetComponent<AudioSource>().Stop();
        }
        PlayerPrefs.SetString("music", musicOn.ToString());
    }

    public void SetSFXToggle()
    {
        print(sfxToggle.isOn.ToString());
        PlayerPrefs.SetString( "sfx", sfxToggle.isOn.ToString());
    }
}
