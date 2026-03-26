using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
    }
    
    private void Update()
    {
        if (gameObject.activeSelf == false) return;
        
        if (input.DialogueMove.UpWasPressedThisFrame)
        {
            choiceIndex--;
            if (choiceIndex < 0) choiceIndex = choicesContainer.childCount - 1;
        }
        else if (input.DialogueMove.DownWasPressedThisFrame)
        {
            choiceIndex++;
            if (choiceIndex >= choicesContainer.childCount) choiceIndex = 0;
        }


        if (input.Progress.WasPressedThisFrame) {
            onSelectChoice?.Invoke(choiceIndex);
            gameObject.SetActive(false);
        }
    }

    // private void UpdateChoiceVisuals()
    // {
    //     GameObject choice = choicesContainer.GetChild(choiceIndex).gameObject;
    //     this.transform.SetParent(choice.transform); // We set the parent to update local position
    //     
    //     TMP_Text txt = choice.GetComponent<TMP_Text>(); // We get the text component, this needs some optimization, i'll do it later.
    //     rectTarget = new Vector2((txt!=null) ? (-txt.textBounds.size.x) : -16f, 8f) + offset; // We set the target position based on the bounds of the current target text.
    //
    //     this.transform.localPosition = Vector2.Lerp(this.transform.localPosition, rectTarget, 15f * Time.deltaTime);
    // }
}