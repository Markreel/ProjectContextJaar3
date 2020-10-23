using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CompositeBehaviour))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CompositeBehaviour _cb = (CompositeBehaviour)target;

        if (_cb.Behaviours == null || _cb.Behaviours.Length == 0)
        {
            EditorGUILayout.HelpBox("No Behaviours in array.", MessageType.Warning);
        }

        else
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Number", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.LabelField("Behaviours", GUILayout.MinWidth(60f));
            EditorGUILayout.LabelField("Weights", GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
            EditorGUILayout.EndHorizontal();

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < _cb.Behaviours.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(i.ToString(), GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                _cb.Behaviours[i] = (FlockBehaviour)EditorGUILayout.ObjectField(_cb.Behaviours[i], typeof(FlockBehaviour), false, GUILayout.MinWidth(60f));
                _cb.Weights[i] = EditorGUILayout.FloatField(_cb.Weights[i], GUILayout.MinWidth(60f), GUILayout.MaxWidth(60f));
                EditorGUILayout.EndHorizontal();
            }
            if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(_cb); }
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Behaviour"))
        {
            AddBehaviour(_cb);
            EditorUtility.SetDirty(_cb);
        }
        EditorGUILayout.EndHorizontal();

        if (_cb.Behaviours != null && _cb.Behaviours.Length > 0)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Remove Behaviour"))
            {
                RemoveBehavior(_cb);
                EditorUtility.SetDirty(_cb);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void AddBehaviour(CompositeBehaviour cb)
    {
        int _oldCount = (cb.Behaviours != null) ? cb.Behaviours.Length : 0;
        FlockBehaviour[] _newBehaviors = new FlockBehaviour[_oldCount + 1];
        float[] _newWeights = new float[_oldCount + 1];

        for (int i = 0; i < _oldCount; i++)
        {
            _newBehaviors[i] = cb.Behaviours[i];
            _newWeights[i] = cb.Weights[i];
        }

        _newWeights[_oldCount] = 1f;
        cb.Behaviours = _newBehaviors;
        cb.Weights = _newWeights;
    }

    private void RemoveBehavior(CompositeBehaviour _cb)
    {
        int _oldCount = _cb.Behaviours.Length;

        if (_oldCount == 1)
        {
            _cb.Behaviours = null;
            _cb.Weights = null;
            return;
        }

        else
        {
            FlockBehaviour[] _newBehaviours = new FlockBehaviour[_oldCount - 1];
            float[] _newWeights = new float[_oldCount - 1];

            for (int i = 0; i < _oldCount - 1; i++)
            {
                _newBehaviours[i] = _cb.Behaviours[i];
                _newWeights[i] = _cb.Weights[i];
            }

            _cb.Behaviours = _newBehaviours;
            _cb.Weights = _newWeights;
        }
    }
}