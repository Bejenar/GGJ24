using UnityEngine;

public class Category : MonoBehaviour
{
    public string category;
    public VideoSO[] videos;

    private CategoryManager _categoryManager;

    private void Awake()
    {
        _categoryManager = FindObjectOfType<CategoryManager>();
    }

    public void PopulatePreviewHolders()
    {
        _categoryManager.PopulatePreviewHolders(videos);
    }
}