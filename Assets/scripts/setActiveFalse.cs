using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setActiveFalse : MonoBehaviour
{
    public GameObject thisGameOb;
    // Start is called before the first frame update
    void Start()
    {
        thisGameOb.SetActive(false);
    }
}
