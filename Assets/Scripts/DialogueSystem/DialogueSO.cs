using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue SO", fileName = "New Dialogue SO")]
public class DialogueSO : ScriptableObject
{
    [SerializeField] private TextAsset inkAsset;
    public TextAsset InkAsset => inkAsset;
}
