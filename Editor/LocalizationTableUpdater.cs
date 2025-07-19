using UnityEngine;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine.Localization.Tables;
using System.IO;
using System.Linq;

public class LocalizationTableUpdater
{
    // 원본 마스터 CSV 파일의 경로
    private const string CSV_PATH = "Resources/CSV/UILocalization.csv";
    // 데이터를 채워넣을 String Table Collection의 이름
    private const string TABLE_COLLECTION_NAME = "MainLocalizationTable";

    // Unity 에디터 상단 메뉴에 "Tools/Update Localization Table from CSV" 항목을 추가합니다.
    [MenuItem("Tools/Update Localization Table from CSV")]
    public static void UpdateTableFromCSV()
    {
        string fullPath = Path.Combine(Application.dataPath, CSV_PATH);

        if (!File.Exists(fullPath))
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {fullPath}");
            return;
        }

        var lines = File.ReadAllLines(fullPath);
        if (lines.Length < 2)
        {
            Debug.LogError("CSV 파일이 비어있거나 헤더만 존재합니다.");
            return;
        }

        var headers = lines[0].Split(',');

        var stringTableCollection = LocalizationEditorSettings.GetStringTableCollection(TABLE_COLLECTION_NAME);
        if (stringTableCollection == null)
        {
            Debug.LogError($"String Table Collection '{TABLE_COLLECTION_NAME}'를 찾을 수 없습니다. 1단계 설정을 확인해주세요.");
            return;
        }

        foreach (var line in lines.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var values = line.Split(',');
            string entryKey = values[0];
            if (string.IsNullOrEmpty(entryKey)) continue;

            for (int i = 1; i < headers.Length; i++)
            {
                string langCode = headers[i];
                // CSV에서 큰따옴표 이스케이프 처리 ("" -> ")
                string value = values[i].Replace("\"\"", "\"");

                var table = stringTableCollection.GetTable(langCode) as StringTable;
                if (table == null)
                {
                    Debug.LogWarning($"'{langCode}'에 해당하는 String Table을 찾을 수 없습니다. 건너뜁니다.");
                    continue;
                }

                var entry = table.GetEntry(entryKey) ?? table.AddEntry(entryKey, value);
                entry.Value = value;

                EditorUtility.SetDirty(table);
                EditorUtility.SetDirty(table.SharedData);
            }
        }

        Debug.Log($"'{TABLE_COLLECTION_NAME}' 테이블 업데이트를 완료했습니다!");
    }
}
