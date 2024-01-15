using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LerpHelpExtensions
{
    public static Coroutine LerpCoroutine(this GameObject gameObj, float time, System.Action<float> block, bool includeZero = false)
    {
        MonoBehaviour behaviour = gameObj.GetComponent<MonoBehaviour>();
        if (behaviour == null)
            return null;

        return behaviour.LerpCoroutine(time, block, includeZero);
    }


    public static Coroutine LerpCoroutine(this MonoBehaviour behaviour, float time, System.Action<float> block, bool includeZero = false)
    {
        return behaviour.StartCoroutine(_LerpCoroutine(time, block, includeZero));
    }

    static IEnumerator _LerpCoroutine(float time, System.Action<float> block, bool includeZero = false)
    {
        if (time <= 0f)
        {
            block(1f);
            yield break;
        }

        float timer = 0f;
        if (includeZero)
        {
            block(0f);
            yield return null;
        }

        while (timer < time)
        {
            timer += Time.deltaTime;
            block(Mathf.Lerp(0f, 1f, timer / time));
            yield return null;
        }
    }

    public static Coroutine AnimateComponent<T>(this MonoBehaviour behaviour, float time, System.Action<T, float> block) where T : Component
    {
        if (block == null)
            return null;

        T component = behaviour.GetComponent<T>();
        if (component == null || !behaviour.gameObject.activeInHierarchy)
            return null;

        return behaviour.StartCoroutine(_LerpCoroutine(time, (timer) =>
        {
            block(component, timer);
        }));

    }
}