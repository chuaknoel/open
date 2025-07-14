/// <summary>
/// 각 데이터 클래스에서 직접 현지화된 텍스트를 가져올 수 있도록 편의 기능을 제공하는 확장 메소드 클래스입니다.
/// </summary>
public static class LocalizationExtensions
{
    // --- WeaponData를 위한 확장 메소드 ---
    //public static string GetName(this WeaponData data)
    //{
    //    if (data == null || string.IsNullOrEmpty(data.NameKey)) return "NAME_KEY_NOT_FOUND";
    //    return LanguageManager.Instance.GetText(data.NameKey);
    //}
    //public static string GetDescription(this WeaponData data)
    //{
    //    if (data == null || string.IsNullOrEmpty(data.DescKey)) return "DESC_KEY_NOT_FOUND";
    //    return LanguageManager.Instance.GetText(data.DescKey);
    //}

    // --- MaterialData를 위한 확장 메소드 ---
    public static string GetName(this MaterialData data)
    {
        if (data == null || string.IsNullOrEmpty(data.NameKey)) return "NAME_KEY_NOT_FOUND";
        return LanguageManager.Instance.GetText(data.NameKey);
    }
    public static string GetDescription(this MaterialData data)
    {
        if (data == null || string.IsNullOrEmpty(data.DescKey)) return "DESC_KEY_NOT_FOUND";
        return LanguageManager.Instance.GetText(data.DescKey);
    }

    // --- CompanionData를 위한 확장 메소드 ---
    public static string GetName(this CompanionData data)
    {
        if (data == null || string.IsNullOrEmpty(data.NameKey)) return "NAME_KEY_NOT_FOUND";
        return LanguageManager.Instance.GetText(data.NameKey);
    }
    public static string GetDescription(this CompanionData data)
    {
        // CompanionData는 DescKey가 SpecDescKey를 사용하므로, 해당 변수를 사용합니다.
        if (data == null || string.IsNullOrEmpty(data.DescKey)) return "DESC_KEY_NOT_FOUND";
        return LanguageManager.Instance.GetText(data.DescKey);
    }

    // (다른 데이터 타입도 필요하다면 여기에 계속 추가하면 됩니다.)
}