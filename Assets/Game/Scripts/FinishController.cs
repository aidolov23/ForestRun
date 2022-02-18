using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<UnitController>())
        {
            PlayerController.instance.Win();
        }
        
    }
}
