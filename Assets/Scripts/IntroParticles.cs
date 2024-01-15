using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroParticles : MonoBehaviour
{

    public GameObject particlePrefab;

    public int numberOfParticles;

    private List<GameObject> particlesPool = new List<GameObject>();


    public int timestamp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InitParticles());
    }

    IEnumerator InitParticles()
    {
        yield return new WaitForSeconds(timestamp);
        for (int i = 0; i < numberOfParticles; i++)
        {
            Vector3 initPos = RandomPositionInit();
            Vector3 endPos = RandomPositionEnd(initPos);
            GameObject part = Instantiate(particlePrefab, initPos, Quaternion.identity, transform);
            particlesPool.Add(part);
            yield return this.AnimateComponent<Transform>(.25f, (t, time) =>
            {
                part.transform.position = Vector3.Lerp(initPos, endPos, time);
            });
            yield return new WaitForSeconds(0.05f);
        }
        yield return null;
    }

    Vector3 RandomPositionInit()
    {
        float posx, posy, posz;
        posx = Random.Range(-40, 40);
        posy = Random.Range(10,20);
        posz = Random.Range(-40, -10);

        return new Vector3(posx, posy, posz);
    }

    Vector3 RandomPositionEnd(Vector3 initPos)
    {
        float posx, posy, posz;
        posz = Random.Range(200, 400);

        return new Vector3(initPos.x, initPos.y, posz);
    }
}
