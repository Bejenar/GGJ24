using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public float typingSpeed = 0.003f; // Adjust typing speed as needed
    private TextMeshProUGUI textComponent;
    private string fullText;

    private ScenarioManager _scenarioManager;
    
    AnimationCurve fadeIn = AnimationCurve.EaseInOut(0, 0, 0.3f, 1);
    
    void Start()
    {
        _scenarioManager = FindObjectOfType<ScenarioManager>();
        // Get the Text component attached to this GameObject
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    public void TypeText(string newText)
    {
        StopCoroutine(TypeTextCoroutine());
        // Store the full text
        fullText = newText;

        // Clear the text and start typing coroutine
        textComponent.text = newText;
        StartCoroutine(TypeTextCoroutine());
    }

    IEnumerator TypeTextCoroutine()
    {
        var timeElapsed = 0f;

        _scenarioManager.canMove = false;
        while (timeElapsed < 0.3f)
        {
            timeElapsed += Time.deltaTime;

            var currentValue = fadeIn.Evaluate(timeElapsed);
            var color = textComponent.color;
            color.a = currentValue;
            textComponent.color = color;
            yield return null;
        }

        _scenarioManager.canMove = true;
    }
}
