using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static UnityEngine.GraphicsBuffer;
[CustomEditor(typeof(EditorLevel))]
public class myeditor : Editor
{
    SerializedProperty blocksInEditorProp;

    void OnEnable()
    {
        // Setup the SerializedProperty
        blocksInEditorProp = serializedObject.FindProperty("BlocksInEditor");
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        // Update the serializedObject to get the latest data
        serializedObject.Update();

        // Draw the default inspector options except for the BlocksInEditor array
        DrawPropertiesExcluding(serializedObject, "BlocksInEditor");

        // Now draw the BlocksInEditor array using the default inspector
        EditorGUILayout.PropertyField(blocksInEditorProp, new GUIContent("Blocks In Editor"), true);

        serializedObject.ApplyModifiedProperties();


        // 获取当前编辑的目标对象
        EditorLevel myScript = (EditorLevel)target;

        // Now draw each BlockInEditor based on its BlockType
        if (myScript.BlocksInEditor != null)
        {
            for (int i = 0; i < myScript.BlocksInEditor.Length; i++)
            {
                EditorGUILayout.LabelField($"Block {i + 1}");
                myScript.BlocksInEditor[i].p1_UpLeft = EditorGUILayout.Vector2IntField("P1 Up Left", myScript.BlocksInEditor[i].p1_UpLeft);
                myScript.BlocksInEditor[i].p2_BottomRight = EditorGUILayout.Vector2IntField("P2 Bottom Right", myScript.BlocksInEditor[i].p2_BottomRight);

                myScript.BlocksInEditor[i].BlockType = (BlockTypeInJson)EditorGUILayout.EnumPopup("Block Type", myScript.BlocksInEditor[i].BlockType);

                // Depending on the BlockType, draw the appropriate directions
                if (myScript.BlocksInEditor[i].BlockType == BlockTypeInJson.XType)
                {
                    // Assuming XDirections is an enum with [Flags] attribute
                    myScript.BlocksInEditor[i].XDirections = (XDirection)EditorGUILayout.EnumFlagsField("X Directions", myScript.BlocksInEditor[i].XDirections);
                }
                else
                {
                    // Assuming IDirections is an enum with [Flags] attribute
                    myScript.BlocksInEditor[i].IDirections = (IDirection)EditorGUILayout.EnumFlagsField("I Directions", myScript.BlocksInEditor[i].IDirections);
                }

                myScript.BlocksInEditor[i].ShowColor = EditorGUILayout.ColorField("Show Color", myScript.BlocksInEditor[i].ShowColor);
            }
        }

        // 如果Inspector界面有任何更改，标记对象为"dirty"以保存更改

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
