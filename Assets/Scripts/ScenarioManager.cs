using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public Canvas canvas;
    public TypewriterEffect dialogueBox;
    public TextMeshProUGUI nameBox;
    public Image sprite;

    public List<CharacterMapEntry> characterList;
    public Dictionary<string, CharacterData> characterMap;

    private string[] scenarioLines;
    private int currentIndex = 0;

    public bool canMove = true;

    AnimationCurve fadeIn = AnimationCurve.EaseInOut(0, 0, 1, 1);
    AnimationCurve fadeOut = AnimationCurve.EaseInOut(0, 1, 1, 0);

    public VideoSO selectedVideo;

    private CameraAnimation _cameraAnimation;
    private ScoreManager _scoreManager;
    private MusicManager _musicManager;

    private bool lastFeedback;

    private string lastTextLine;
    public TextMeshProUGUI hint;

    public GameObject secretButton;

    private HashSet<string> servedCustomers = new();

    public AudioClip clip;

    private void Start()
    {
        _musicManager = FindObjectOfType<MusicManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _cameraAnimation = FindObjectOfType<CameraAnimation>();
        LoadScenarioFile("scenario_file");
        characterMap = CharacterMapEntry.ToDictionary(characterList);
        Debug.Log(scenarioLines.Length);
    }

    void Update()
    {
        // Check for left mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Parse next command when clicked
            if (currentIndex < scenarioLines.Length)
            {
                ParseNextCommand();
            }
        }
    }

    private IEnumerator PlaySpriteAnimation(AnimationCurve curve)
    {
        var timeElapsed = 0f;

        canMove = false;
        while (timeElapsed < 1)
        {
            timeElapsed += Time.deltaTime;

            var currentValue = curve.Evaluate(timeElapsed);
            var color = sprite.color;
            color.a = currentValue;
            sprite.color = color;
            yield return null;
        }

        canMove = true;
    }

    // Function to load scenario file from Resources folder
    private void LoadScenarioFile(string fileName)
    {
        // Load scenario file from Resources folder
        TextAsset scenarioFile = Resources.Load<TextAsset>("Scenario/" + fileName);
        if (scenarioFile != null)
        {
            // Split the text asset's contents into lines
            scenarioLines = scenarioFile.text.Split('\n');
        }
        else
        {
            Debug.LogError("Scenario file not found: " + fileName);
        }
    }

    // Function to parse the next command in the scenario
    private void ParseNextCommand()
    {
        if (canMove && currentIndex < scenarioLines.Length)
        {
            string line = scenarioLines[currentIndex];
            string[] parts = line.Split(' ');
            string command = parts[0];
            string characterID = parts[1].Trim();

            if (!characterMap.ContainsKey(characterID))
            {
                Debug.LogError("Character with ID " + characterID + " not found.");
                return;
            }

            CharacterData characterData = characterMap[characterID];

            switch (command)
            {
                case "show":
                    ShowCharacter(characterData);
                    break;
                case "text":
                    string[] textParts = line.Split(new[] { '"' }, StringSplitOptions.RemoveEmptyEntries);
                    string text = textParts[1];
                    DisplayText(characterData, text);
                    break;
                case "gameplay":
                    StartGameplay(characterData);
                    break;
                case "feedback":
                    string positiveFeedback = GetFeedbackText(line, 1);
                    string negativeFeedback = GetFeedbackText(line, 3);
                    DisplayFeedback(characterData, positiveFeedback, negativeFeedback);
                    break;
                case "hide":
                    HideCharacter(characterData);
                    break;
                case "musicstop":
                    StopMusic();
                    break;
                case "gameover":
                    GameOver();
                    break;
                default:
                    Debug.LogError("Unknown command: " + command);
                    break;
            }

            currentIndex++;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game over!");
        _musicManager.TogglePause();
        SceneManager.LoadScene("Win");
    }

    private void StopMusic()
    {
        _musicManager.TogglePause();
        dialogueBox.TypeText("");
        nameBox.text = "";
    }

    // Function to handle gameplay event completion
    public void OnGameplayEventComplete(VideoSO videoSo)
    {
        selectedVideo = videoSo;
        _cameraAnimation.MoveToLeft();
        canMove = true;
        _musicManager.PlayNext();
        _musicManager.TogglePause();
        ParseNextCommand(); // Parse next command after gameplay event is finished
    }

    // Function to extract feedback text from command line
    private string GetFeedbackText(string line, int index)
    {
        string[] parts = line.Split('"');
        return parts[index];
    }

    void ShowCharacter(CharacterData characterData)
    {
        Debug.Log("Show character" + characterData.characterName);
        // Get character data from scriptable object and show the character sprite
        sprite.sprite = characterData.characterSprite;
        StartCoroutine(PlaySpriteAnimation(fadeIn));
    }

    void HideCharacter(CharacterData characterData)
    {
        Debug.Log("Hide character " + characterData.characterName);
        // Fade out the character sprite
        StartCoroutine(PlaySpriteAnimation(fadeOut));
        AudioSource.PlayClipAtPoint(clip, Vector2.zero);
    }

    void DisplayText(CharacterData characterData, string text)
    {
        Debug.Log("Display text" + text);
        // Get character data from scriptable object and display text in dialogue box
        dialogueBox.TypeText(text);
        nameBox.text = characterData.characterName;
        nameBox.color = characterData.textColor;
        nameBox.alpha = 1;

        lastTextLine = text;
    }

    void StartGameplay(CharacterData characterData)
    {
        hint.text = lastTextLine;
        
        if (characterData.characterName == "???")
        {
            secretButton.SetActive(true);
        }
        canMove = false;
        Debug.Log("Start gameplay");
        _musicManager.TogglePause();
        _cameraAnimation.MoveToRight();
        // Trigger gameplay event
        // Call OnGameplayEventComplete when gameplay event is finished
    }

    void DisplayFeedback(CharacterData characterData, string positiveFeedback, string negativeFeedback)
    {
        Debug.Log("Gameplay feedback " + positiveFeedback + negativeFeedback);
        // Check gameplay result

        if (characterData.characterName.Equals("You"))
        {
            string text = lastFeedback ? positiveFeedback : negativeFeedback;
            DisplayText(characterData, text);
            return;
        }

        if (!characterData.favMemesMap.ContainsKey(selectedVideo.videoID))
        {
            DisplayText(characterData, negativeFeedback);
            lastFeedback = false;
            return;
        }

        bool goodResult = characterData.favMemesMap[selectedVideo.videoID] >= 50;
        // if (characterData.characterName == "???")
        // {
        //     goodResult = !goodResult;
        // }

        if (!servedCustomers.Contains(characterData.characterName))
        {
            _scoreManager.AddScore(characterData.favMemesMap[selectedVideo.videoID]);
        }
        servedCustomers.Add(characterData.characterName);
        
        lastFeedback = goodResult;
        string feedbackText = goodResult ? positiveFeedback : negativeFeedback;
        // Display feedback text
        DisplayText(characterData, feedbackText);
    }
}

[Serializable]
public class CharacterMapEntry
{
    public string characterID;
    public CharacterData characterData;

    public static Dictionary<string, CharacterData> ToDictionary(List<CharacterMapEntry> wrapper)
    {
        Dictionary<string, CharacterData> characterMap = new Dictionary<string, CharacterData>();
        foreach (var entry in wrapper)
        {
            characterMap[entry.characterID] = entry.characterData;
        }

        return characterMap;
    }
}