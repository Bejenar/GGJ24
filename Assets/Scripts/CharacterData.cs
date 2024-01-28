using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Character", menuName = "Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    public Color textColor;
    public Sprite characterSprite;
    public List<MemeData> favoriteMemes = new List<MemeData>();

    public Dictionary<string, int> favMemesMap = new();

    private void OnEnable()
    {
        favMemesMap = ToDictionary(favoriteMemes);
    }
    
    public static Dictionary<string, int> ToDictionary(List<MemeData> wrapper)
    {
        Dictionary<string, int> memesMap = new Dictionary<string, int>();
        foreach (var entry in wrapper)
        {
            memesMap[entry.id] = entry.rating;
        }
        return memesMap;
    }
}

[System.Serializable]
public class MemeData
{
    public string id;
    public int rating;
}