using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    private void Start()
    {
        System.Random r = new System.Random(1);

        Debug.Log(r.Next(0, 10));
        Debug.Log(r.Next(0, 5));
        Debug.Log(r.Next(0, 10));
    }
}
