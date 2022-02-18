using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRespawnsController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject unit;

    void Start()
    {
        foreach(Transform resp in transform)
        {
            if (Random.Range(0,4)  == 1)
            {
                Instantiate(unit, resp.position, resp.rotation);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
