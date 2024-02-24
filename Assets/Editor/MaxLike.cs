using Codice.Client.BaseCommands.BranchExplorer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class MaxLike : Editor
{



    static MaxLike()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    static int direction = 0;
    static int updownDirection = 0;
    static Transform[] selectedTransforms;
    static Vector3[] originalPositions;
    static Quaternion[] originalRotations;
    public static bool pressedShift = false;
    public static bool createdTemp = false;
    public static List<GameObject> tempItems = new List<GameObject>();
    public static List<GameObject> OrignalGameObjects = new List<GameObject>();
    public static void ResetAll()
    {
        selectedTransforms = null;
        originalPositions = null;
        originalRotations = null;
        pressedShift = false;
        createdTemp = false;
        foreach (var item in tempItems)
        {
            DestroyImmediate(item);
        }
        tempItems.Clear();

    }
    private static void OnSceneGUI(SceneView sceneView)
    {


        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.alt && e.control)
        {

            //  Debug.Log(sceneView.orthographic);
            switch (e.keyCode)
            {
                case KeyCode.LeftArrow:
                    direction -= 1;
                    break;
                case KeyCode.RightArrow:
                    direction += 1;
                    break;
                case KeyCode.UpArrow:
                    updownDirection -= 1;
                    break;
                case KeyCode.DownArrow:
                    updownDirection += 1;
                    break;

                default:
                    return;
            }

            UpdateOrientation(sceneView);

            e.Use();
        }

        else
        {
            #region Instantiation

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftShift)
            {
                pressedShift = true;
            }

            if (e.type == EventType.KeyUp && e.keyCode == KeyCode.LeftShift)
            {
                pressedShift = false;
            }

            if (e.type == EventType.MouseDown && e.button == 0 && pressedShift && !createdTemp)
            {

                createdTemp = true;
                OrignalGameObjects.Clear();
                foreach (var item in Selection.transforms)
                {
                    OrignalGameObjects.Add(item.gameObject);

                    var clone = Instantiate(item.gameObject, item.transform.position, item.transform.rotation);
                    tempItems.Add(clone);
                }
                originalPositions = tempItems.Select(obj => obj.transform.position).ToArray();
                originalRotations = tempItems.Select(obj => obj.transform.rotation).ToArray();
                Selection.objects = tempItems.ToArray();
                selectedTransforms = tempItems.Select(s => s.transform).ToArray();


            }

            else if (e.type == EventType.MouseUp && e.button == 0 && pressedShift && createdTemp)
            {
                pressedShift = false;
                createdTemp = false;
                #region show Window
                CustomInstantiationWindow window = EditorWindow.GetWindow<CustomInstantiationWindow>();
                #endregion
            }
            else if (e.type == EventType.MouseUp && createdTemp && pressedShift == false)
            {
                pressedShift = false;
                createdTemp = false;
                Selection.objects = MaxLike.OrignalGameObjects.ToArray();
                ResetAll();

            }
            #endregion

        }

    }

    public static void MakeCopys(int num)
    {
        pressedShift = false;
        Vector3[] deltaPositions = selectedTransforms
            .Select((transform, index) => transform.position - originalPositions[index])
            .ToArray();
        Quaternion[] deltaRotations = selectedTransforms
        .Select((transform, index) => transform.rotation * Quaternion.Inverse(originalRotations[index]))
        .ToArray();
        List<GameObject> clonedData = new List<GameObject>();

        for (int i = 0; i < num; i++)
        {
            for (int j = 0; j < selectedTransforms.Length; j++)
            {
                // Record the current state of the object for undo
                Undo.RecordObject(selectedTransforms[j].gameObject, "Make Copy");

                Quaternion rotValue = originalRotations[j] * deltaRotations[j];
                if (deltaRotations[j] == Quaternion.identity)
                {
                    rotValue = originalRotations[j];
                }
                else
                {
                    var rot = rotValue.eulerAngles * (i + 1);
                    rotValue = Quaternion.Euler(rot);
                }

                GameObject duplicatedObject = Instantiate(selectedTransforms[j].gameObject, originalPositions[j] + deltaPositions[j] * (i + 1), rotValue);
                clonedData.Add(duplicatedObject);

                // Register the creation of the object for undo
                Undo.RegisterCreatedObjectUndo(duplicatedObject, "Create Object");
            }
        }
        OrignalGameObjects.AddRange(clonedData);
        ResetAll();
    }



    private static void UpdateOrientation(SceneView sceneView)
    {

        if (direction > 3 || direction < -3)
        {
            direction = 0;
        }
        if (updownDirection > 3 || updownDirection < -3)
        {
            updownDirection = 0;
        }
        int angle = (direction * 90) % 360;
        int updownAngle = (updownDirection * 90) % 360;
        sceneView.LookAt(sceneView.pivot, Quaternion.Euler(-updownAngle, -angle, 0), sceneView.size, sceneView.orthographic);
    }
}
public class CustomInstantiationWindow : EditorWindow
{
    public int numberOfInstances;
    Vector3 rot = Vector3.zero;
    Vector3 pos = Vector3.zero;

    private void OnGUI()
    {
        GUILayout.Label("Set Number of Instances:");
        numberOfInstances = EditorGUILayout.IntField(numberOfInstances);

        if (MaxLike.tempItems.Count > 0)
        {
            if (rot == Vector3.zero)
            {
                rot = MaxLike.tempItems[0].transform.rotation.eulerAngles;
            }
            if (pos == Vector3.zero)
            {
                pos = MaxLike.tempItems[0].transform.position;

            }
            rot = EditorGUILayout.Vector3Field("Delta Rotation", rot);
            pos = EditorGUILayout.Vector3Field("Delta Position", pos);
        }



        if (GUILayout.Button("OK"))
        {
            if (rot != Vector3.zero)
            {
                MaxLike.tempItems.First().transform.rotation = Quaternion.Euler(rot);
            }
            if (pos != Vector3.zero)
            {
                MaxLike.tempItems.First().transform.position = pos;
            }

            if (numberOfInstances != 0)
            {
                MaxLike.MakeCopys(numberOfInstances);
            }

            Close(); // call destroy
        }
    }

    private void OnDestroy()
    {
        Selection.objects = MaxLike.OrignalGameObjects.ToArray();
        DestroyTemData();
    }

    public static void DestroyTemData()
    {
        MaxLike.ResetAll();
    }
}