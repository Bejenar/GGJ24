using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemeTubeInitializer : MonoBehaviour
{
    public Category Category;
    
    private void Start()
    {
        Category.PopulatePreviewHolders();
    }
}
