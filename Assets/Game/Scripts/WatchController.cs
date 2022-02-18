using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchController : MonoBehaviour
{

    public GameObject axeMan;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != axeMan && other.gameObject.GetComponent<UnitController>() && other.gameObject.GetComponent<UnitController>().isMain)
        {
            PlayerController.instance.AddUnit(axeMan);
            Destroy(gameObject); //уничтожаем свою watchZone
        }
    }   
}
