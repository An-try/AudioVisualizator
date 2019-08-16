using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MicrophoneInput : MonoBehaviour
{
    private AudioSource audioSource;

    private string selectedMicrophone;

    public AudioMixerGroup audioMixerGroupMaster;
    public AudioMixerGroup audioMixerGroupMicrophone;

    public bool useMicrophone;

    public int deviceIndex = 0;

    void Start()
    {
        audioSource = Visualizator.instance.GetComponent<AudioSource>();

        if (useMicrophone)
        {
            if(Microphone.devices.Length > deviceIndex) // Если к компьютеру подключено устройство записи звука под заданым индексом
            {
                selectedMicrophone = Microphone.devices[deviceIndex].ToString();
                audioSource.outputAudioMixerGroup = audioMixerGroupMicrophone;
                audioSource.clip = Microphone.Start(selectedMicrophone, true, 1, AudioSettings.outputSampleRate);
            }
            else
            {
                useMicrophone = false;
            }
        }
        audioSource.Play();
    }
}
