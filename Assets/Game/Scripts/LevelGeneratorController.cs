using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LevelStructure
{
    planeClear, planeWall, planeRotateLeft, planeRotateRight, finish
}

public class LevelGeneratorController : MonoBehaviour
{
    [SerializeField]
    GameObject planeWall;
    [SerializeField]
    GameObject planeClear;
    [SerializeField]
    GameObject planeRotateLeft;
    [SerializeField]
    GameObject planeRotateRight;
    [SerializeField]
    GameObject finish;

    
    int x;
    int z;

    int direction; //0 - вперед, 1 - вправо

    [SerializeField]
    LevelStructure[] levelStructures;

    // Start is called before the first frame update
    void Start()
    {
        x = 0;
        z = 0;
        direction = 0;

        Quaternion newQuat = Quaternion.identity;

        foreach(LevelStructure ls in levelStructures)
        {
            newQuat = Quaternion.Euler(0, direction, 0);
            //место новой плитки
            if(direction == 0)
            {
                z++;
            }else if (direction == 90)
            {
                x++;
            }
            else if (direction == -90)
            {
                x--;
            }
            else if (Mathf.Abs(direction) == 180)
            {
                z--;
            }


            GameObject newObj = planeClear;

            switch (ls)
            {
                case LevelStructure.planeClear:
                    newObj = planeClear;
                    break;
                case LevelStructure.planeWall:
                    newObj = planeWall;
                    break;

                case LevelStructure.planeRotateLeft:
                    newObj = planeRotateLeft;
                    direction -= 90;
                    break;

                case LevelStructure.planeRotateRight:
                    newObj = planeRotateRight;
                    direction += 90;
                    break;

                case LevelStructure.finish:
                    newObj = finish;
                    break;
            }
            

                
            Vector3 position = new Vector3(10 * x, 0, 10 * z);
            Instantiate(newObj, position, newQuat);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
