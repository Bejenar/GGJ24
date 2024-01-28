using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "New Video", menuName = "VideoSO")]
public class VideoSO : ScriptableObject
{
    public string videoID;
    public string title;
    public Sprite preview;
    public string[] tags;
    public VideoClip clip;
}
