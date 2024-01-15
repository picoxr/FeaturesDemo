using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSender : MonoBehaviour
{
    public GameObject particlePrefab;

    public List<GameObject> referencePositions;

    private List<GameObject> particlesPool = new List<GameObject>();

    public float timestamp;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(InitParticles());
    }

    public IEnumerator InitParticles()
    {
        //yield return new WaitForSeconds(timestamp);
        for (int i = 0; i < referencePositions.Count; i++)
        {
            Vector3 refPos = referencePositions[i].transform.position;
            Vector3 initPos = InitPosition(refPos);
            Vector3 endPos = EndingPosition(refPos);
            GameObject part = Instantiate(particlePrefab, initPos, Quaternion.identity, transform);
            particlesPool.Add(part);
            yield return this.AnimateComponent<Transform>(.03f, (t, time) =>
            {
                part.transform.position = Vector3.Lerp(initPos, refPos, time);
            });

        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < referencePositions.Count; i++)
        {
            Vector3 refPos = referencePositions[i].transform.position;
            Vector3 initPos = InitPosition(refPos);
            Vector3 endPos = EndingPosition(refPos);

            yield return this.AnimateComponent<Transform>(.025f, (t, time) =>
            {
                particlesPool[i].transform.position = Vector3.Lerp(refPos, endPos, time);
            });
        }

        yield return new WaitForSeconds(6f);

        foreach (GameObject part in particlesPool)
        {
            Destroy(part); 
        }
        yield return null;
    }

    Vector3 InitPosition(Vector3 initFurther)
    {
        float posx, posy, posz;
        posx = initFurther.x;
        posy = initFurther.y;
        posz = initFurther.z - 300f;

        return new Vector3(posx, posy, posz);
    }

    Vector3 EndingPosition(Vector3 refPos)
    {
        float posx, posy, posz;
        posx = refPos.x;
        posy = refPos.y;
        posz = refPos.z + 300f;

        return new Vector3(posx, posy, posz);
    }
}
