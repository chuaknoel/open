using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecentDataParam : OpenParam
{
    public string[] recentItems = new string[2];
    public string[] recentQuests = new string[2];
    public string level;

    public RecentDataParam(string[] _recentItems, string[] _recentQuests, string _level)
    {
        this.recentItems = _recentItems;
        this.recentQuests = _recentQuests;
        this.level = _level;
    }
}
