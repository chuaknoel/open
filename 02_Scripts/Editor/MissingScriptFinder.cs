using UnityEngine;
using UnityEditor;


//씬이나 프리팹에 missing 된 스크립트를 찾아주는 에디터입니다.
//Tools -> Find Missing Scripts -> In Scene : 현재 씬에 배치된 오브젝트를 탐색하여 missing 된 스크립트를 가지는 오브젝트 탐색
//Tools -> Find Missing Scripts -> In Prefabs : 프로젝트 폴더를 순회하여 missing 된 스크립트를 가지는 프리팹 파일 탐색
public class MissingScriptFinder : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts/In Scene")]
    static void FindMissingScriptsInScene()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        int count = 0;

        foreach (GameObject go in allObjects)
        {
            if (go.hideFlags != HideFlags.None)
                continue;

            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Logger.LogError($"[Scene] Missing script found in: {GetHierarchyPath(go)}");
                    count++;
                }
            }
        }

        Logger.Log($"[Scene] 총 {count}개의 Missing Script가 발견되었습니다.");
    }

    [MenuItem("Tools/Find Missing Scripts/In Prefabs")]
    static void FindMissingScriptsInPrefabs()
    {
        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");
        int count = 0;

        foreach (string guid in allPrefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            var components = prefab.GetComponentsInChildren<Component>(true);

            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] == null)
                {
                    Logger.LogError($"[Prefab] Missing script in: {path}");
                    count++;
                    break; // 하나만 발견해도 출력 후 넘어감
                }
            }
        }

        Logger.Log($"[Prefab] 총 {count}개의 프리팹에 Missing Script가 있습니다.");
    }

    static string GetHierarchyPath(GameObject go)
    {
        string path = go.name;
        while (go.transform.parent != null)
        {
            go = go.transform.parent.gameObject;
            path = go.name + "/" + path;
        }
        return path;
    }
}