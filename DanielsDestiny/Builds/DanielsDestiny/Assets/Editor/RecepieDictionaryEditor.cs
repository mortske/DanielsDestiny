using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CraftingDictionary))]
public class RecepieDictionaryEditor : Editor 
{
    List<bool> foldoutPos;

    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();
        DrawNewInspector();
    }

    void DrawNewInspector()
    {

        CraftingDictionary myTarget = (CraftingDictionary)target;

        if (foldoutPos == null)
        {
            foldoutPos = new List<bool>();
            for (int i = 0; i < myTarget.recepies.Count; i++)
                foldoutPos.Add(false);
        }

        EditorGUILayout.LabelField("Recepielength: " + myTarget.recepies.Count);

        for (int i = 0; i < myTarget.recepies.Count; i++)
        {
            string name = "recepie " + i;
            if (myTarget.recepies[i].recepieName != "")
            {
                name = myTarget.recepies[i].recepieName;
            }
            foldoutPos[i] = EditorGUILayout.Foldout(foldoutPos[i], name);
            if (foldoutPos[i])
            {
                EditorGUI.indentLevel++;

                myTarget.recepies[i].recepieName = EditorGUILayout.TextField(myTarget.recepies[i].recepieName);

                if (myTarget.recepies[i].items != null)
                {
                    for (int j = 0; j < myTarget.recepies[i].items.Count; j++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField("item: ", GUILayout.MaxWidth(50));
                        myTarget.recepies[i].items[j] = (GameObject)EditorGUILayout.ObjectField(myTarget.recepies[i].items[j], typeof(GameObject));
                        EditorGUILayout.LabelField("amnt: ", GUILayout.MaxWidth(55));
                        myTarget.recepies[i].amount[j] = EditorGUILayout.IntField(myTarget.recepies[i].amount[j]);

                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("ResultItem: ");
                GameObject result2 = myTarget.recepies[i].result;
                myTarget.recepies[i].result = (GameObject)EditorGUILayout.ObjectField(result2, typeof(GameObject));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Add item"))
                {
                    if (myTarget.recepies[i].items == null)
                    {
                        myTarget.recepies[i].items = new List<GameObject>();
                        myTarget.recepies[i].amount = new List<int>();
                    }
                    myTarget.recepies[i].items.Add(null);
                    myTarget.recepies[i].amount.Add(0);
                }
                if (GUILayout.Button("Remove item"))
                {
                    myTarget.recepies[i].items.RemoveAt(myTarget.recepies[i].items.Count - 1);
                    myTarget.recepies[i].amount.RemoveAt(myTarget.recepies[i].amount.Count - 1);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add"))
        {
            myTarget.recepies.Add(new Recepie());
            foldoutPos.Add(false);
        }
        if (GUILayout.Button("Remove"))
        {
            myTarget.recepies.RemoveAt(myTarget.recepies.Count - 1);
            foldoutPos.RemoveAt(foldoutPos.Count - 1);
        }
        EditorGUILayout.EndHorizontal();
    }
}

