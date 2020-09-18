using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConveyerBelt))]
public class ConveyerBeltEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ConveyerBelt belt = (ConveyerBelt)target;

        base.OnInspectorGUI();

        if (PrefabUtility.GetOutermostPrefabInstanceRoot(belt.gameObject) == null)
        {
            belt.BuildConveyerBelt(belt.conveyerBeltLength);
            belt.SetBeltSpeed(belt.beltSpeed);
        }

            belt.conveyerBeltLength = (int)EditorGUILayout.Slider("Conveyor Belt Length", belt.conveyerBeltLength, 3, 50);
            belt.beltSpeed = EditorGUILayout.Slider("Belt Speed", belt.beltSpeed, 1, 20);

        


        
    }

    public void OnSceneGUI()
    {
        ConveyerBelt belt = (ConveyerBelt)target;

        if (PrefabUtility.GetOutermostPrefabInstanceRoot(belt.gameObject) != null)
        {

            PrefabUtility.UnpackPrefabInstance(instanceRoot: belt.gameObject, PrefabUnpackMode.Completely, InteractionMode.AutomatedAction);
        }

    }

    




}
