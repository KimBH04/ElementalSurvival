using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Dice))]
[CanEditMultipleObjects]
public class DiceEditor : Editor
{
    private new Dice target;

    private void OnEnable()
    {
        target = base.target as Dice;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying)
        {
            if (GUILayout.Button("Roll"))
            {
                target.Roll();
            }
            if (GUILayout.Button("Roll All"))
            {
                foreach (var item in FindObjectsByType<Dice>(FindObjectsSortMode.None))
                {
                    item.Roll();
                }
            }
        }
    }
}
