using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VolumeControlVideo : MonoBehaviour
{
    public Slider volumeSlider;
    public VideoPlayer VideoPlayer;

    void Start()
    {
        VideoPlayer = FindAnyObjectByType<VideoManager>().GetComponent<VideoPlayer>();

        volumeSlider.value = 0.4f;

        // Ensure that the slider and audio source are properly set in the Unity Editor
        if (volumeSlider == null || VideoPlayer == null)
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
        VideoPlayer.SetDirectAudioVolume(0, newValue);
    }
}