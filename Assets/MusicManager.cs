using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioSource audioSource;
    private int currentSongIndex = 0;
    private bool isPaused = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        Application.runInBackground = true;
        // Start playing the first song
        PlaySong(0);

        // Start the loop coroutine
        StartCoroutine(PlayNextSong());
    }

    // Coroutine to play the next song in a loop
    private IEnumerator PlayNextSong()
    {
        while (true)
        {
            // Wait until the current song finishes playing
            while (audioSource.isPlaying || isPaused)
            {
                yield return new WaitForSeconds(1);
            }

            // Move to the next song index

            PlayNext();
            // Play the next song

        }
    }

    void PlaySong(int index)
    {
        // Set the clip to the specified song and play it
        audioSource.clip = songs[index];
        audioSource.Play();
    }

    public void PlayNext()
    {
        currentSongIndex = (currentSongIndex + 1) % songs.Length;
        PlaySong(currentSongIndex);
    }

    public void TogglePause()
    {
        // Toggle the pause state
        isPaused = !isPaused;

        // If not paused, resume playing the current song
        if (!isPaused)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Pause();
        }
    }
}
