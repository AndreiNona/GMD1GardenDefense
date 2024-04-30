using System;
using System.Collections;
using System.Collections.Generic;
using Path;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
[CustomEditor(typeof(FollowPath))]
public class FollowPathEditor : Editor
{
 private FollowPath targetComponent;

 private void OnSceneGUI()
 {
     targetComponent = (FollowPath) target;
     Handles.color = Color.cyan;
     var positions = targetComponent.path;
     for (int i = 1; i < positions.Length + 1; i++)
     {
         var previousPoint = positions[i - 1];
         var currentPoint = positions[i % positions.Length];
         Handles.DrawDottedLine(previousPoint,currentPoint,4f);
     }

     var polygonPoints = new Vector3[positions.Length];
     for (int i = 0; i < positions.Length; i++)
     {
         polygonPoints[i] = positions[i];
         positions[i] = Handles.PositionHandle(positions[i], Quaternion.identity);
     }
     Handles.color = new Color(0, 1, 1, .2f);
     Handles.DrawAAConvexPolygon(polygonPoints);
     if (GUI.changed)
     {
         EditorUtility.SetDirty(targetComponent);
         EditorSceneManager.MarkSceneDirty(targetComponent.gameObject.scene);
     }
 }
 
}
