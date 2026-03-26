using System;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEditor;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public static bool isDialogueActive;
    
    [SerializeField] private Typewriter typewriter;
    [SerializeField] private DialogueCursor dialogueCursor;
    
    [SerializeField] private DialogueSO testDialogueSo;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject choicesBox;
    [SerializeField] private TMP_Text[] choiceTexts; 
    [SerializeField] private GameObject continueIndicator;
    
    
    private Story currentStory;
    private bool canProgressDialogue;

    public void Update()
    {
        if (InputManager.Instance.InputReader.DebugA.WasPressedThisFrame)
        {
            DisplayDialogue(testDialogueSo);
        }
    }


    public void DisplayDialogue(DialogueSO dialogueSo)
    {
        InputManager.Instance.InputReader.EnableInput(InputActionType.Dialogue);
        isDialogueActive = true;
        
        choicesBox.SetActive(false);
        dialogueBox.SetActive(true);
        continueIndicator.SetActive(false);
        
        canProgressDialogue = true;
        currentStory = new Story(dialogueSo.InkAsset.text);
        ContinueStory();
    }
    
    private void ContinueStory()
    {
        if (!currentStory) return;
        if (!canProgressDialogue) return;

        if (currentStory.canContinue)
        {
            continueIndicator.SetActive(false);
            canProgressDialogue = false; 
            typewriter.onTextComplete += OnTypewriterTextComplete;
            typewriter.DisplayText(currentStory.Continue());
        }
        else
            EndDialogue();
    }

    private void EndDialogue()
    {
        InputManager.Instance.InputReader.EnableInput(InputActionType.Player);
        isDialogueActive = false;
        
        currentStory = null;
        dialogueBox.SetActive(false);

        CleanupEvents();
    }
    
    private bool TryDisplayChoices()
    {
        List<Choice> choices = currentStory.currentChoices;

        if (choices.Count == 0) return false; 
        if (choices.Count > choiceTexts.Length) return false;

        choicesBox.SetActive(true);
        
        for (int i = 0; i < choices.Count; i++)
            choiceTexts[i].text = choices[i].text;

        dialogueCursor.Enable();
        dialogueCursor.onSelectChoice += OnCursorSelectChoice;
        
        return true;
    }

    private void OnTypewriterTextComplete()
    {
        continueIndicator.SetActive(true);
        typewriter.onTextComplete -= OnTypewriterTextComplete;
        
        canProgressDialogue = !TryDisplayChoices();
        continueIndicator.SetActive(canProgressDialogue);
    }

    private void OnCursorSelectChoice(int index)
    {
        dialogueCursor.onSelectChoice -= OnCursorSelectChoice;
        choicesBox.SetActive(false);
        dialogueCursor.gameObject.SetActive(false);

        canProgressDialogue = true;
        currentStory.ChooseChoiceIndex(index);
        ContinueStory();
    }

    private void OnDestroy()
    {
        CleanupEvents();
    }

    private void CleanupEvents()
    {
        typewriter.onTextComplete -= OnTypewriterTextComplete;
        dialogueCursor.onSelectChoice -= OnCursorSelectChoice;
    }
}