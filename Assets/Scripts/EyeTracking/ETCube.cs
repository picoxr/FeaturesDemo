using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETCube : ETObject
{

    float focusedTime = 0f;
    float activateTime = 2f;

    private GameObject clickWindow;
    public GameObject clickWindowPrefab;

    public override void IsFocused()
    {
        base.IsFocused();
        focusedTime += Time.deltaTime;

        Debug.Log("FOCUSED TIME " + focusedTime);

        if (focusedTime >= activateTime)
        {
            Debug.Log("activate window!");

            if (!clickWindow)
            {
                clickWindow = Instantiate(clickWindowPrefab);
                Debug.Log("instantiate window");

            }
            else if (!clickWindow.activeInHierarchy)
            {
                clickWindow.SetActive(true);
                Debug.Log("activate window");
            }
        }
            


    }

    public override void UnFocused()
    {
        base.UnFocused();

        activateTime = 0;
        Debug.Log("reset window");
    }
}