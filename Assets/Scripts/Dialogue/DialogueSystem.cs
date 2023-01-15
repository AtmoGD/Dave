using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(DialogueSystem))]
public class DialogueSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueSystem myScript = (DialogueSystem)target;
        if (GUILayout.Button("Next Sentence"))
        {
            myScript.NextSentence();
        }

        if (GUILayout.Button("Start Dialogue"))
        {
            myScript.StartDialogue();
        }
    }
}
#endif

[Serializable]
public class Dialogue
{
    public List<Sentence> sentences = new List<Sentence>();
}

[Serializable]
public class Sentence
{
    public Sprite sprite;
    public string name;
    public string text;
    public float typingSpeed;
}

public class DialogueSystem : UIMenuController
{
    public TextMeshProUGUI textDisplay = null;
    public Dialogue dialogue = null;
    public GameObject continueButton;

    private int currentSentenceIndex = 0;
    private int currentLetterIndex = 0;
    private float currentTime = 0f;

    public bool isTyping = false;

    public override void Start()
    {
        base.Start();

        textDisplay.text = "";
        continueButton.SetActive(false);
        isTyping = false;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (textDisplay.text != dialogue.sentences[currentSentenceIndex].text && isTyping)
            Type();
    }

    public void StartDialogue()
    {
        SetIsActive(true);
        isTyping = true;
    }

    public void Type()
    {
        if (currentTime > dialogue.sentences[currentSentenceIndex].typingSpeed)
        {
            currentTime = 0f;
            textDisplay.text += dialogue.sentences[currentSentenceIndex].text[currentLetterIndex];
            currentLetterIndex++;

            if (textDisplay.text == dialogue.sentences[currentSentenceIndex].text)
            {
                continueButton.SetActive(true);
                isTyping = false;
            }
        }
    }

    public void NextSentence()
    {
        continueButton.SetActive(false);
        currentLetterIndex = 0;
        textDisplay.text = "";
        isTyping = true;

        if (currentSentenceIndex < dialogue.sentences.Count - 1)
            currentSentenceIndex++;
        else
            EndDialogue();

    }

    public void EndDialogue()
    {
        SetIsActive(false);
        isTyping = false;
    }
}
