using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChinaLocalizationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("System Language:" + Application.systemLanguage.ToString());

        switch (Application.systemLanguage)
        {
            case SystemLanguage.Chinese:
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Chinese");
                break;
            case SystemLanguage.ChineseTraditional:
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Chinese");
                break;
            case SystemLanguage.ChineseSimplified:
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll("Chinese");
                break;
            default:
                Lean.Localization.LeanLocalization.SetCurrentLanguageAll(Application.systemLanguage.ToString());
                break;
        }
    }
}
