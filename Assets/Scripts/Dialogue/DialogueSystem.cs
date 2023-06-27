using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

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
public enum DialogeSide
{
    Left,
    Right
}

[Serializable]
public class Sentence
{
    public Sprite sprite;
    public DialogeSide side;
    public string name;
    public string text;
    public float typingSpeed;
}

public class DialogueSystem : UIMenuController
{
    public TextMeshProUGUI textDisplay = null;
    public TextMeshProUGUI nameDisplay = null;
    public Image imageLeft = null;
    public Image imageRight = null;
    public Dialogue dialogue = null;
    public UIMenuItem continueButton;
    public TMP_Text continueButtonText;

    private int currentSentenceIndex = 0;
    private int currentLetterIndex = 0;
    private float currentTime = 0f;

    public bool isTyping = false;

    [SerializeField] private UnityEngine.Events.UnityEvent onDialogueEnd = null;

    public override void Awake()
    {
        base.Awake();

        textDisplay.text = "";
        // continueButton.gameObject.SetActive(false);
        imageLeft.gameObject.SetActive(false);
        imageRight.gameObject.SetActive(false);
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
        currentSentenceIndex = 0;
        nameDisplay.text = dialogue.sentences[currentSentenceIndex].name;
        UpdateImage();
        isTyping = true;
    }

    public void UpdateImage()
    {
        if (dialogue.sentences[currentSentenceIndex].side == DialogeSide.Left)
        {
            imageLeft.sprite = dialogue.sentences[currentSentenceIndex].sprite;
            imageLeft.gameObject.SetActive(true);
            imageRight.gameObject.SetActive(false);
        }
        else
        {
            imageRight.sprite = dialogue.sentences[currentSentenceIndex].sprite;
            imageRight.gameObject.SetActive(true);
            imageLeft.gameObject.SetActive(false);
        }
    }

    public void Type()
    {
        if (currentTime > dialogue.sentences[currentSentenceIndex].typingSpeed)
        {
            currentTime = 0f;
            textDisplay.text += dialogue.sentences[currentSentenceIndex].text[currentLetterIndex];
            currentLetterIndex++;
            continueButton.Deselect();
            continueButtonText.text = "";

            if (textDisplay.text == dialogue.sentences[currentSentenceIndex].text)
            {
                continueButton.Select();
                continueButtonText.text = "Next...";
                continueButton.gameObject.SetActive(true);
                isTyping = false;
            }
        }
    }

    public void NextSentence()
    {
        // continueButton.gameObject.SetActive(false);
        currentLetterIndex = 0;
        textDisplay.text = "";
        isTyping = true;

        if (currentSentenceIndex < dialogue.sentences.Count - 1)
        {
            currentSentenceIndex++;
            nameDisplay.text = dialogue.sentences[currentSentenceIndex].name;
            UpdateImage();
        }
        else
            EndDialogue();
    }

    public void EndDialogue()
    {
        imageRight.gameObject.SetActive(false);
        imageLeft.gameObject.SetActive(false);
        SetIsActive(false);
        isTyping = false;
        currentSentenceIndex = 0;
        onDialogueEnd?.Invoke();
    }
}
