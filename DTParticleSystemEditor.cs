using UnityEditor;
using UnityEngine;

[ExecuteAlways]
#if UNITY_EDITOR
[CustomEditor(typeof(DTParticleSystem))]
public class DTParticleSystemEditor : Editor
{
    DTGUIs guis = new DTGUIs();
    public override void OnInspectorGUI()
    {
        var dtps = target as DTParticleSystem;

        if (GUILayout.Button("Play"))
        {
            dtps.Start();
        }

        dtps.triggered = GUILayout.Toggle(dtps.triggered, "Triggered");
        if (dtps.triggered)
        {
            guis.MinMaxSlider("Amount:", ref dtps.triggeredAmount, true);
            guis.Slider("Duration:", ref dtps.duration);
        }

        GUILayout.Space(15);
        guis.Slider("Area:", ref dtps.area);
        guis.Slider("Frequency:", ref dtps.frequency);
        guis.MinMaxSlider("Speed:", ref dtps.speed);
        guis.MinMaxSlider("LifeTime:", ref dtps.lifeTime);
        guis.Slider("Rotation", ref dtps.rotation);

        GUILayout.Space(15);
        dtps.randomDirection = GUILayout.Toggle(dtps.randomDirection, "Random direction");
        if (!dtps.randomDirection)
        {
            dtps.direction = EditorGUILayout.Vector3Field("Direction:", dtps.direction);
            guis.Slider("Direction variance:", ref dtps.directionVariance);
        }

        GUILayout.Space(15);
        dtps.gravity = GUILayout.Toggle(dtps.gravity, "Gravity");
        if (dtps.gravity)
        {
            guis.Slider("Gravity scale:", ref dtps.gravityScale);
            dtps.gravityDirection = EditorGUILayout.Vector3Field("Gravity direction:", dtps.gravityDirection);
        }

        GUILayout.Space(15);
        guis.Slider("Drag:", ref dtps.drag);

        GUILayout.Space(15);
        guis.MinMaxSlider("Scale:", ref dtps.scale);
        guis.MinMaxSlider("Growth:", ref dtps.growth);
        dtps.growThenShrink = GUILayout.Toggle(dtps.growThenShrink, "Grow then shrink");

        dtps.OnValidate();
    }
}
#endif
