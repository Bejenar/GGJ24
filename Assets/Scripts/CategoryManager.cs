using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    public VideoPreviewHolder[] previewHolders;

    public void PopulatePreviewHolders(VideoSO[] videos)
    {
        for (int i = 0; i < Mathf.Min(videos.Length, previewHolders.Length); i++)
        {
            previewHolders[i].SetVideoInfo(videos[i]);
        }
    }
}
