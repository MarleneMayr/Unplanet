#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform[] goalTransforms;
    public GoalLocation[] goalLocations;
    public ScreenEffect[] effects;

    public int index { get; private set; }
    public bool isLastGoal() => index >= goalLocations.Length;

    public void Init()
    {
        index = 0;
    }

    public GoalLocation GetNextGoal()
    {
        var next = goalLocations[index];
        effects[index]?.Activate();
        index++;
        return next;
    }

    public void DeactivateEffects()
    {
        foreach (var e in effects)
        {
            e?.Deactivate();
        }
    }

    [System.Serializable]
    public class GoalLocation
    {
        public Vector3 pos = Vector3.zero;
        public Quaternion rot = Quaternion.identity;
        public Vector3 scl = Vector3.one;

        public GoalLocation(Transform t)
        {
            pos = t.position;
            rot = t.rotation;
            scl = t.lossyScale;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Level))]
    [CanEditMultipleObjects]
    public class LevelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var level = (Level)target;
            if (GUILayout.Button("save goal transforms") && level.goalTransforms.Length > 0)
            {
                SaveTransforms(level);
            }
            if (GUILayout.Button("clear goal transforms"))
            {
                level.goalTransforms = new Transform[0];
                level.goalLocations = new GoalLocation[0];
                ApplyArrayChanges(level, level.goalTransforms);
                ApplyArrayChanges(level, level.goalLocations);
            }
            if (level.goalLocations.Length != level.goalTransforms.Length)
            {
                EditorGUILayout.HelpBox("Don't forget to save when done!", MessageType.Warning);
            }
            if (level.effects.Length != level.goalTransforms.Length)
            {
                EditorGUILayout.HelpBox("Effects have to be assigned per goal index! Match Amount!", MessageType.Error);
            }
        }

        private void SaveTransforms(Level level)
        {
            level.goalLocations = new GoalLocation[level.goalTransforms.Length];
            for (int i = 0; i < level.goalTransforms.Length; i++)
            {
                level.goalLocations[i] = new GoalLocation(level.goalTransforms[i]);
            }
            ApplyArrayChanges(level, level.goalLocations);
        }

        private void ApplyArrayChanges(Level level, GoalLocation[] newArray)
        {
            level.goalLocations = newArray;
            // apply overrides if prefab instance was edited
            if (PrefabUtility.IsPartOfAnyPrefab(level))
            {
                PrefabUtility.ApplyPrefabInstance(level.gameObject, InteractionMode.UserAction);
            }

            // apply changes if prefab was edited in prefab scene
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }

        private void ApplyArrayChanges(Level level, Transform[] newArray)
        {
            level.goalTransforms = newArray;
            // apply overrides if prefab instance was edited
            if (PrefabUtility.IsPartOfAnyPrefab(level))
            {
                PrefabUtility.ApplyPrefabInstance(level.gameObject, InteractionMode.UserAction);
            }

            // apply changes if prefab was edited in prefab scene
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null)
            {
                EditorSceneManager.MarkSceneDirty(prefabStage.scene);
            }
        }
    }
#endif
}