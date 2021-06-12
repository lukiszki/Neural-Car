using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CarController))]
public class SaveProgress : Editor
{
    public override void OnInspectorGUI()
    {
        CarController con = (CarController)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Save generation"))
        {
            con.Save();
        }
    }
}
