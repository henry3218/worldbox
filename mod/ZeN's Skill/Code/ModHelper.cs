// ModHelper.cs
using System.Collections.Generic;
using UnityEngine;

// 這是包含所有輔助函式的靜態類別
public static class ModHelper
{
    // 這個方法可以讓我們更方便地檢查單位是否擁有 HashSet 中的任何一個狀態
    public static bool hasStatus(this Actor pActor, HashSet<string> pStatusIDs)
    {
        foreach (string statusID in pStatusIDs)
        {
            if (pActor.hasStatus(statusID))
            {
                return true;
            }
        }
        return false;
    }
}