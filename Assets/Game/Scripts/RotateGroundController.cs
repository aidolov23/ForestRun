using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GroundRotation
{
    Left, Right
}

public class RotateGroundController : MonoBehaviour
{
    [SerializeField]
    GroundRotation rotation;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<UnitController>() && other.gameObject.GetComponent<UnitController>().isMain)
        {
            Vector3 rotateDirection;
            if (rotation == GroundRotation.Left)
            {
                rotateDirection = transform.right * -1;
            }
            else
            {
                rotateDirection = transform.right;
            }
            
            PlayerController.instance.RotateMainDirection(rotateDirection);
        }
    }
}
