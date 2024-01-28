using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    public Canvas playerCanvas;
    public Canvas selectCanvas;

    private ScenarioManager _scenarioManager;

    private MusicManager _musicManager;

    private VideoSO video;

    public TextMeshProUGUI title;

    private void Awake()
    {
        _scenarioManager = FindObjectOfType<ScenarioManager>();
        _musicManager = FindObjectOfType<MusicManager>();
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += StopVideo;
    }

    public void StopVideo(VideoPlayer source)
    {
        source.Stop();
        selectCanvas.gameObject.SetActive(true);
        playerCanvas.gameObject.SetActive(false);
        if (_scenarioManager != null)
        {
            _scenarioManager.OnGameplayEventComplete(video);
        }

        if (SceneManager.GetActiveScene().name.Equals("Cinema"))
        {
            _musicManager.TogglePause();
        }
    }

    public void PlayVideo(VideoSO video)
    {
        if (SceneManager.GetActiveScene().name.Equals("Cinema"))
        {
            _musicManager.TogglePause();
        }

        this.video = video;
        title.text = video.title;
        playerCanvas.gameObject.SetActive(true);
        selectCanvas.gameObject.SetActive(false);
        _videoPlayer.clip = video.clip;
        _videoPlayer.Play();
    }
}