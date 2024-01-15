using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_DissolveOverTime : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    private float t = 0.0f;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(Undissolve(meshRenderer.material));
    }


    IEnumerator Undissolve(Material controllerMat)
    {
        yield return new WaitForSeconds(4);
        float speed = .25f;
        Material mats = controllerMat;
        while (mats.GetFloat("_Cutoff") > 0)
        {
            mats.SetFloat("_Cutoff", Mathf.Cos(t * speed));
            t += Time.deltaTime;

            yield return null;
        }
        yield return null;
    }
}
