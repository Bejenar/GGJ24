using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoPreviewHolder : MonoBehaviour
{
    private Image previewImage;
    private TextMeshProUGUI titleText;
    private TextMeshProUGUI tags;

    public VideoSO video;

    private VideoManager _videoManager;

    private void Awake()
    {
        _videoManager = FindObjectOfType<VideoManager>();
        previewImage = this.transform.Find("Thumbnail").GetComponent<Image>();
        titleText = this.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        tags = this.transform.Find("Tags").GetComponent<TextMeshProUGUI>();
    }

    public void SetVideoInfo(VideoSO v)
    {
        this.video = v;
        previewImage.sprite = v.preview;
        titleText.text = v.title;
        tags.text = string.Join(", ", v.tags);
    }

    public void PlayVideo()
    {
        if (video.clip == null)
        {
            return;
        }

        _videoManager.PlayVideo(video);
        Debug.Log("playing video");
    }
}