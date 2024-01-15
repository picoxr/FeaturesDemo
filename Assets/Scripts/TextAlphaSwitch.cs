using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAlphaSwitch : MonoBehaviour
{

    public bool isVisible = false;

    public void SwitchVisibility()
    {
        if (!isVisible)
        {
            StartCoroutine(FadeTextToFullAlpha(1f, GetComponent<TMPro.TextMeshProUGUI>()));
        }
        else
        {
            StartCoroutine(FadeTextToZeroAlpha(1f, GetComponent<TMPro.TextMeshProUGUI>()));
        }
        isVisible = !isVisible;
    }



    public IEnumerator FadeTextToFullAlpha(float t, TMPro.TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }

    public IEnumerator FadeTextToZeroAlpha(float t, TMPro.TextMeshProUGUI i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
