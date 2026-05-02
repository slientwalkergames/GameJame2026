using UnityEngine;

[CreateAssetMenu(fileName = "New Thought", menuName = "Elias/Thought Node")]
public class ThoughtNode : ScriptableObject {
    [TextArea(3, 10)] public string thoughtText;
}