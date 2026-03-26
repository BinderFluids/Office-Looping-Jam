using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private InputReader input;
    
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private int charactersPerSecond;
    [SerializeField] private int skipCharactersPerSecond;
    [SerializeField] private float punctuationDelay;
    
    private int currentVisibleCharacters;
    private readonly HashSet<char> punctuation = new() {  '!', '?', ',' };

    Coroutine typewriterCoroutine;
    private WaitForSeconds waitForPunctuation;

    public event Action onTextComplete = delegate { };
    
    private void Awake()
    {
        waitForPunctuation = new WaitForSeconds(punctuationDelay);
    }

    public void DisplayText(string text)
    {
        dialogueText.maxVisibleCharacters = 0; 
        dialogueText.text = text;
        
        if (typewriterCoroutine != null) StopCoroutine(typewriterCoroutine);
        typewriterCoroutine = StartCoroutine(TypewriterEffect());
    }
    
    IEnumerator TypewriterEffect()
    {
        int previousPunctuationIndex = 0;
        int nextPunctuationIndex = int.MaxValue; 
        while (dialogueText.maxVisibleCharacters < dialogueText.text.Length - 1)
        {
            for (int i = previousPunctuationIndex; i < dialogueText.maxVisibleCharacters; i++)
            {
                if (punctuation.Contains(dialogueText.text[i]))
                {
                    nextPunctuationIndex = i + 1;
                    break;
                }
            }
            
            dialogueText.maxVisibleCharacters =
                input.Progress.IsPressed
                    ? dialogueText.maxVisibleCharacters + Mathf.CeilToInt(Time.deltaTime * skipCharactersPerSecond)
                    : Mathf.Min(
                        nextPunctuationIndex,
                        dialogueText.maxVisibleCharacters + Mathf.CeilToInt(Time.deltaTime * charactersPerSecond)
                    );
            dialogueText.maxVisibleCharacters = Mathf.Min(dialogueText.maxVisibleCharacters, dialogueText.text.Length);
            char currentChar = dialogueText.text[dialogueText.maxVisibleCharacters - 1];
            
            if (
                punctuation.Contains(currentChar) && 
                dialogueText.maxVisibleCharacters != dialogueText.text.Length - 1 &&
                !Input.GetKey(KeyCode.Space)
                )  
            {
                previousPunctuationIndex = nextPunctuationIndex;
                nextPunctuationIndex = int.MaxValue;
                yield return waitForPunctuation;
            }
            else
                yield return null;
        }
        
        dialogueText.maxVisibleCharacters = dialogueText.text.Length;
        
        typewriterCoroutine = null;
        onTextComplete?.Invoke();
    }
}
