using System;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityUtils;

public class DialogueCursor : MonoBehaviour
{
    [SerializeField] private InputReader input; 
    [SerializeField] private RectTransform choicesContainer;
    [SerializeField] private int choiceIndex;
    
    public event Action<int> onSelectChoice = delegate { };
    
    public Vector2 offset;
    Vector2 rectTarget;


    public void Enable()
    {
        choiceIndex = 0; 
        //UpdateChoiceVisuals(); 
        gameObject.SetActive(true);
        UpdateChoiceVisuals();
    }
    
    private void Update()
    {
        if (gameObject.activeSelf == false) return;
        
        int activeChildCount = choicesContainer.Children().Count(c => c.gameObject.activeSelf);
        if (input.DialogueMove.UpWasPressedThisFrame)
        {
            choiceIndex--;
            if (choiceIndex < 0) choiceIndex = activeChildCount - 1;
        }
        else if (input.DialogueMove.DownWasPressedThisFrame)
        {
            choiceIndex++;
            if (choiceIndex >= activeChildCount) choiceIndex = 0;
        }

        UpdateChoiceVisuals();

        if (input.Progress.WasPressedThisFrame) {
            onSelectChoice?.Invoke(choiceIndex);
            gameObject.SetActive(false);
        }
    }

    private void UpdateChoiceVisuals()
    {
        GameObject choice = choicesContainer.GetChild(choiceIndex).gameObject;
        transform.SetParent(choice.transform); // We set the parent to update local position
        
        TMP_Text txt = choice.GetComponent<TMP_Text>(); // We get the text component, this needs some optimization, i'll do it later.
        rectTarget = new Vector2((txt!=null) ? (-txt.textBounds.size.x) : -16f, 8f) + offset; // We set the target position based on the bounds of the current target text.
    
        transform.localPosition = Vector2.Lerp(transform.localPosition, rectTarget, 15f * Time.deltaTime);
    }
}