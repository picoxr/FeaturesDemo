using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Mirror;

    public Vector3 initPos;
    public Vector3 endPos;

    public void ActivateMirror()
    {
        Mirror.SetActive(true);
        StartCoroutine(SpawnMirror());
        
    }

    public IEnumerator SpawnMirror()
    {
        float duration = .4f;

        yield return this.AnimateComponent<Transform>(duration, (t, time) =>
        {
            Mirror.transform.localPosition = Vector3.Lerp(initPos, endPos, time);
        });
        Mirror.GetComponent<AudioSource>().Play();

        yield return null;
    }

    
}
