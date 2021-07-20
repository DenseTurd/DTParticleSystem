using UnityEditor;
using UnityEngine;

public class DTGUIs 
{
    public void Slider(string label, ref Vector3 vector)
    {
        Slider(label, ref vector, false);
    }

    public void Slider(string label, ref Vector3 vector, bool integer)
    {
        string rounding = integer ? "F0" : "F2";
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        vector.x = EditorGUILayout.FloatField(float.Parse(vector.x.ToString(rounding)), GUILayout.Width(35));
        vector.x = GUILayout.HorizontalSlider(vector.x, vector.y, vector.z);
        GUILayout.EndHorizontal();
    }

    public void MinMaxSlider(string label, ref Vector4 vector)
    {
        MinMaxSlider(label,ref vector, false);
    }

    public void MinMaxSlider(string label,ref Vector4 vector, bool integer)
    {
        EditorGUI.BeginChangeCheck();

        // Put temp values in the gui elements so they don't alter the actual value
        float minVal = vector.x;
        float maxVal = vector.y;

        // Draw it
        string rounding = integer ? "F0" : "F2";
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(label);
        minVal = EditorGUILayout.FloatField(float.Parse(minVal.ToString(rounding)), GUILayout.Width(35));
        EditorGUILayout.MinMaxSlider(ref minVal, ref maxVal, vector.z, vector.w);
        maxVal = EditorGUILayout.FloatField(float.Parse(maxVal.ToString(rounding)), GUILayout.Width(35));
        GUILayout.EndHorizontal();

        // Constrain it
        if (minVal < vector.z)
        {
            minVal = vector.z;
        }

        if (maxVal > vector.w)
        {
            maxVal = vector.w;
        }

        // Set value, also more constraints
        if (EditorGUI.EndChangeCheck())
        {
            vector = new Vector4(minVal > maxVal ? maxVal : minVal, maxVal, vector.z, vector.w);
        }
    }
}
