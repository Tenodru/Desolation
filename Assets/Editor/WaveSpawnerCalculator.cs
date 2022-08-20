using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaveSpawner))]
public class WaveSpawnerCalculator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WaveSpawner waveSpawner = (WaveSpawner)target;

        EditorGUILayout.LabelField("Spawn Tier: " + waveSpawner.UISpawnTier());
        EditorGUILayout.LabelField("Time Scale: " + waveSpawner.UITimeScale());
        EditorGUILayout.LabelField("Spawn Budget: " + waveSpawner.UICalculateSpawnBudget());
    }
}
