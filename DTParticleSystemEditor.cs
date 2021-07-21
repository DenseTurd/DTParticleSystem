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

        EditorUtility.SetDirty(target);
        GUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel(new GUIContent("Particle", "Any ole gameObject"));
        dtps.prefab = (Component)EditorGUILayout.ObjectField(dtps.prefab, typeof(Component), true);
        GUILayout.EndHorizontal();

        dtps.triggered = GUILayout.Toggle(dtps.triggered, new GUIContent("Triggered", "Is the particle system constant or triggered"));
        if (dtps.triggered)
        {
            if (GUILayout.Button("Trigger", GUILayout.Width(100)))
            {
                dtps.Trigger();
            }
            guis.MinMaxSlider(new GUIContent("Amount:", $"Upper limit of {dtps.triggeredAmount.w} for performance"), ref dtps.triggeredAmount, true);
            guis.Slider(new GUIContent("Duration:", $"Lower limit of {dtps.duration.y} for performance"), ref dtps.duration);
        }

        GUILayout.Space(15);
        guis.Slider(new GUIContent("Area:", $"Min: {dtps.area.y}, Max: {dtps.area.z}"), ref dtps.area);
        guis.Slider(new GUIContent("Frequency:", $"Upper limit of {dtps.frequency.z} for performance"), ref dtps.frequency);
        guis.MinMaxSlider(new GUIContent("Speed:", $"Min: {dtps.speed.z}, Max: {dtps.speed.w}"), ref dtps.speed);
        guis.MinMaxSlider(new GUIContent("LifeTime:", $"Min: {dtps.lifeTime.z}, Max: {dtps.lifeTime.w}"), ref dtps.lifeTime);
        guis.Slider(new GUIContent("Rotation", "Randomly rotates particles by up to this many degrees"), ref dtps.rotation);

        GUILayout.Space(15);
        dtps.randomDirection = GUILayout.Toggle(dtps.randomDirection, new GUIContent("Random direction", "Uncheck to set your own direction"));
        if (!dtps.randomDirection)
        {
            dtps.direction = EditorGUILayout.Vector3Field("Direction:", dtps.direction);
            guis.Slider(new GUIContent("Direction variance:", "Introduce some randomness to the inital direction"), ref dtps.directionVariance);
        }

        GUILayout.Space(15);
        dtps.gravity = GUILayout.Toggle(dtps.gravity, new GUIContent("Gravity", "Apply a force in a direction"));
        if (dtps.gravity)
        {
            guis.Slider(new GUIContent("Gravity scale:", "The amount of force applied"), ref dtps.gravityScale);
            dtps.gravityDirection = EditorGUILayout.Vector3Field("Gravity direction:", dtps.gravityDirection);
        }

        GUILayout.Space(15);
        guis.Slider(new GUIContent("Drag:", $"Min: {dtps.drag.y}, Max: {dtps.drag.z}. Try negative drag ;)"), ref dtps.drag);

        GUILayout.Space(15);
        guis.MinMaxSlider(new GUIContent("Scale:", "Size of particles"), ref dtps.scale);
        guis.MinMaxSlider(new GUIContent("Growth:", "Changes size of particle relative to scale over lifetime"), ref dtps.growth);
        dtps.growThenShrink = GUILayout.Toggle(dtps.growThenShrink, new GUIContent("Grow then shrink", "Scales particle from smallest growth value, to largest and back over lifetime"));

        dtps.OnValidate();
    }
}
#endif
