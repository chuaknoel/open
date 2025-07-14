/// <summary>
/// CSV로부터 생성되는 모든 데이터 클래스의 기반이 되는 인터페이스입니다.
/// 이 인터페이스를 구현하는 모든 데이터는 반드시 고유 식별자인 'ID'를 가져야 한다는
/// 최소한의 규칙만을 강제합니다.
/// </summary>
public interface IGetCSVData
{
    /// <summary>
    /// 데이터의 고유 식별자 Key입니다. (예: "WEAPON_001")
    /// </summary>
    string ID { get; set; }
}
