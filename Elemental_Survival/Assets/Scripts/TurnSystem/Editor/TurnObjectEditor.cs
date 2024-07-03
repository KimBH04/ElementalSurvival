using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TurnObject))]
[CanEditMultipleObjects]
public class TurnObjectEditor : Editor
{
    private TurnObject turn;

    private void OnEnable()
    {
        turn = (TurnObject)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (EditorApplication.isPlaying)
        {
            if (!turn.IsTurnEnd)
            {
                if (GUILayout.Button("Turn End"))
                {
                    turn.TurnEnd();
                }
            }
        }
    }
}
