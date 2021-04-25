//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace WorldWarOneTools
//{
//    [CustomEditor(typeof(MovablePlatform), true)]
//    public class PathEditor : Editor
//    {
//        protected virtual void OnSceneGUI()
//        {
//            if (!Application.isPlaying)
//            {
//                Handles.color = Color.green;
//                MovablePlatform platform = target as MovablePlatform;

//                int next = new int();

//                for (int i = 0; i < platform.numberOfPaths.Count; i++)
//                {
//                    EditorGUI.BeginChangeCheck();

//                    Vector3 position = platform.numberOfPaths[i];

//                    next = i + 1;
//                    if (next == platform.numberOfPaths.Count)
//                        next = 0;

//                    Vector3 newPosition = Handles.FreeMoveHandle(position, Quaternion.identity, 2, new Vector3(.5f, .5f, .5f), Handles.CircleHandleCap);

//                    Handles.DrawDottedLine(platform.numberOfPaths[i], platform.numberOfPaths[next], 5);

//                    Handles.Label(platform.numberOfPaths[i], platform.gameObject.name + i.ToString());

//                    if (EditorGUI.EndChangeCheck())
//                    {
//                        Undo.RecordObject(target, "MoveablePlatform");
//                        platform.numberOfPaths[i] = newPosition;
//                    }
//                }
//            }
//        }
//    }

//}

