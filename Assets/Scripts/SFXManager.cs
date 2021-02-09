using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [SerializeField] Toggle audioToggle;

    private void Start()
    {
        if (AudioListener.volume == 0)
        {
            audioToggle.isOn = false;
        }
    }

}
