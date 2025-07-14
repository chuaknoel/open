using UnityEngine;

[System.Serializable]
public struct DialogueLine
{
    public string SpeakerName;
    [TextArea(3, 5)] // 인스펙터에서 여러 줄로 편하게 입력할 수 있도록 합니다.
    public string Text;
}