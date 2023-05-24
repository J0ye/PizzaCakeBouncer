using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManagerAndSettings : MenuManager
{
    public AudioMixer master;
    public Dropdown resolutionDropDown;

    private Resolution[] reso;

    public override void Start()
    {
        UpdateUI();
        SetUpResolutionDropDown();
    }

    public void AdjustAudioVolume(float val)
    {
        master.SetFloat("Volume", val);
    }

    public void AdjustResolution(int val)
    {
        Resolution newReso = reso[val];
        Screen.SetResolution(newReso.width, newReso.height, Screen.fullScreen);
    }

    private void SetUpResolutionDropDown()
    {
        reso = Screen.resolutions;

        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();
        int currenResolutionIndex = 0;
        for(int i = 0; i < reso.Length; i++)
        {
            Debug.Log("Resolution: " + reso[i].width + "x" + reso[i].height);
            string option = reso[i].width + "x" + reso[i].height;
            options.Add(option);

            if(reso[i].EqualTo(Screen.currentResolution))
            {
                currenResolutionIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
    }
}
