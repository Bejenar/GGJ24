using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioSource audioSource;

    void Start()
    {
        audioSource = FindAnyObjectByType<MusicManager>().GetComponent<AudioSource>();
        
        volumeSlider.value = audioSource.volume;
        
        // Ensure that the slider and audio source are properly set in the Unity Editor
        if (volumeSlider == null || audioSource == null)
        {
            Debug.LogError("VolumeSlider or AudioSource is not set in the inspector!");
            return;
        }

        // Add a listener to the slider to detect changes in its value
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }

    void OnVolumeChanged(float newValue)
    {
        // Update the volume of the audio source based on the slider value
        audioSource.volume = newValue;
    }
}
