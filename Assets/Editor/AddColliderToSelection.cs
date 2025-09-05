// Assets/Editor/AddColliderToSelection.cs
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AddColliderToSelection : EditorWindow
{
    const int BATCH_SIZE = 200;       // ここを小さくすると更に安全
    bool useUndo = false;              // Undoを使うか（大量処理はOFF推奨）
    bool convex = false;              // 建物/道路は通常 false
    bool includeChildren = true;      // 子も対象
    bool onlyIfMissing = true;        // 既に付いてたらスキップ

    [MenuItem("Tools/Colliders/Add MeshCollider (Selection)")]
    static void Open() => GetWindow<AddColliderToSelection>("Add MeshCollider");

    void OnGUI()
    {
        EditorGUILayout.LabelField("Target", EditorStyles.boldLabel);
        includeChildren = EditorGUILayout.Toggle("Include Children", includeChildren);
        onlyIfMissing = EditorGUILayout.Toggle("Only If Missing", onlyIfMissing);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
        convex = EditorGUILayout.Toggle("Convex", convex);
        useUndo = EditorGUILayout.Toggle("Use Undo (safe for small sets)", useUndo);

        if (GUILayout.Button("Run"))
            Run();
    }

    void Run()
    {
        var roots = Selection.gameObjects;
        if (roots == null || roots.Length == 0)
        {
            EditorUtility.DisplayDialog("No Selection", "Hierarchyで対象を選択してください。", "OK");
            return;
        }

        // 対象収集
        var targets = new List<GameObject>();
        foreach (var r in roots)
        {
            if (includeChildren)
                foreach (var t in r.GetComponentsInChildren<Transform>(true)) targets.Add(t.gameObject);
            else
                targets.Add(r);
        }

        // 重複除去
        var seen = new HashSet<GameObject>();
        targets.RemoveAll(go => !seen.Add(go));

        int processed = 0, added = 0;
        try
        {
            for (int i = 0; i < targets.Count; i += BATCH_SIZE)
            {
                int end = Mathf.Min(i + BATCH_SIZE, targets.Count);

                int group = 0;
                if (useUndo)
                {
                    Undo.SetCurrentGroupName("Add MeshColliders (Batch)");
                    group = Undo.GetCurrentGroup();
                }

                for (int j = i; j < end; j++)
                {
                    var go = targets[j];
                    EditorUtility.DisplayProgressBar("Adding MeshColliders", go.name, (float)j / targets.Count);

                    var mf = go.GetComponent<MeshFilter>();
                    if (!mf || !mf.sharedMesh) { processed++; continue; }

                    var existing = go.GetComponent<MeshCollider>();
                    if (onlyIfMissing && existing) { processed++; continue; }

                    MeshCollider mc;
                    if (useUndo)
                        mc = Undo.AddComponent<MeshCollider>(go);
                    else
                        mc = go.AddComponent<MeshCollider>();

                    mc.sharedMesh = mf.sharedMesh;
                    mc.convex = convex;
#if UNITY_2020_2_OR_NEWER
                    mc.cookingOptions = MeshColliderCookingOptions.EnableMeshCleaning |
                                        MeshColliderCookingOptions.UseFastMidphase;
#endif
                    added++; processed++;
                }

                if (useUndo) Undo.CollapseUndoOperations(group);   // バッチ毎に畳む
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
            // シーン変更フラグを立てる（保存ダイアログのため）
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }

        Debug.Log($"[AddColliderToSelection] 追加: {added} / 処理: {processed} / 収集: {targets.Count}");
    }
}
