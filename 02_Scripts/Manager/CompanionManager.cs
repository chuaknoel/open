using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CompanionManager : MonoBehaviour
{
    public static CompanionManager Instance { get; private set; }
    private Dictionary<string, CompanionData> companionDatas = new();
    private Dictionary<string, Companion> MyCompanions = new();

    public Companion testCompanion;

    private void Awake()
    {
        if (Instance != null)
        {
            Logger.LogError($"[YourManager] Duplicate instance detected on '{gameObject.name}'. " +
                        $"This may indicate a missing unload or unintended duplicate. Destroying this instance.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public async Task Init()
    {
        companionDatas = DataManager.Instance.CompanionDB;

        testCompanion?.TestInit();

        foreach (var companionData in companionDatas)
        {
            //Logger.Log($"{companionData.Value.CompanionPrefabAddress} : {companionData.Value.IsJoined}");
            if (!companionData.Value.IsJoined) continue;
            
            GameObject companionObj = Instantiate(await AddressableManager.Instance.LoadAsset<GameObject>(companionData.Value.CompanionPrefabAddress));
            if (companionObj.TryGetComponent<Companion>(out Companion companion))
            {
                companion.Init();                
                companion.gameObject.SetActive(false);
                MyCompanions[companionData.Value.ID] = companion;
            }
        }
    }

    public async void JoinParty(string ID)
    {
        companionDatas[ID].IsJoined = true;
        GameObject companionObj = Instantiate(await AddressableManager.Instance.LoadAsset<GameObject>(companionDatas[ID].CompanionPrefabAddress));
        if (companionObj.TryGetComponent<Companion>(out Companion companion))
        {
            companion.Init();
            companion.gameObject.SetActive(false);          
            MyCompanions[ID] = companion;
        }
    }

    public void SetBattleScene()
    {
        foreach (var companion in MyCompanions)
        {
            companion.Value.StartBattle();
        }
    }

    public void EndBattleScene()
    {
        foreach (var companion in MyCompanions)
        {
            companion.Value.EndBattle();
        }
    }

    public void UnLoad()
    {
        if (Instance == this)
        {
            Instance = null;
        }
        else if (Instance == null)
        {
            Logger.LogError($"[YourManager] UnLoad called, but Instance was already null. Possible duplicate unload or uninitialized state.");
        }
        else
        {
            Logger.LogError($"[YourManager] UnLoad called by a non-instance object: {gameObject.name}. Current Instance is on {Instance.gameObject.name}");
        }
    }
}
