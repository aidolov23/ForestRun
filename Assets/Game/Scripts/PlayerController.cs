using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    public Camera cam;
    Vector3 camOffset;

    [SerializeField] private float smoothTime = 0.5f;
    private Vector3 followerVelocity = Vector3.zero;

    Queue<UnitController> units;
    public GameObject mainAxeMan;
    UnitController mainAxeManController;

    public GameObject latestUnit;

    [SerializeField]
    GameObject winPanel;

    [SerializeField]
    GameObject gameOverPanel;

    [SerializeField]
    Text unitsCountText;

    Vector3 newDirection;


    // Start is called before the first frame update
    void Start()
    {
        newDirection = transform.forward; //новое направление совпадает с текущим

        latestUnit = mainAxeMan;

        instance = this;

        mainAxeManController = mainAxeMan.GetComponent<UnitController>();

        units = new Queue<UnitController>();
        //главный юнит уже не в очереди

        camOffset = cam.transform.position - mainAxeMan.transform.position;

        mainAxeManController.SetStatus(1);
        mainAxeManController.isMain = true;

        Destroy(mainAxeManController.watchZone.gameObject);

        UpdateUI();

    }

    // Update is called once per frame
    void Update()
    {
        if (mainAxeMan)
        {
            //transform.position = mainAxeMan.transform.position; //объем всегда в месте главного отряда, нужно для поворотов

            //когда есть главный, то выраниваем камеру на него
            float camHOffset = camOffset.z;
            float camVOffset = camOffset.y;
            Vector3 currentOffset = transform.forward * camHOffset + transform.up * camVOffset;

            Vector3 newCameraPosition = mainAxeMan.transform.position + currentOffset;
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position, newCameraPosition, ref followerVelocity, smoothTime);


            Quaternion targetRot = Quaternion.LookRotation(-currentOffset);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, targetRot, 2f * Time.deltaTime);

            //cam.transform.LookAt(mainAxeMan.transform);
        }

        if(newDirection != transform.forward)
        {
            Quaternion directionRot = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, directionRot, 1f * Time.deltaTime);
        }
             
    }

    public void Win()
    {
        Time.timeScale = 0f;
        winPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AtackTarget(GameObject target)
    {
        StartCoroutine(Atack(target));
    }
    

    IEnumerator Atack(GameObject target)
    {
        
        mainAxeMan.GetComponent<UnitController>().SetStatus(2);
        yield return new WaitForSeconds(1.0f);

        int restLives = target.GetComponent<TreeController>().GetDamage();

        if (units.TryDequeue(out var newMainAxemanController)){

            newMainAxemanController.target = mainAxeMan.GetComponent<UnitController>().target;
            newMainAxemanController.isMain = true;
            newMainAxemanController.SetStatus(1);

            mainAxeMan.GetComponent<UnitController>().Death();
            UpdateUI();

            mainAxeMan = newMainAxemanController.gameObject;
            if(restLives == 0)
            {
                //цель после уничтожения, пройти это вырубленное место
                mainAxeMan.GetComponent<UnitController>().targetMovePosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                //mainAxeMan.GetComponent<UnitController>().targetMovePosition = Vector3.zero;
                mainAxeMan.GetComponent<UnitController>().target = null;

                yield return new WaitForSeconds(1f);
                mainAxeMan.GetComponent<UnitController>().SetStatus(1);
            }
        }
        else
        {
            //Game Over
            mainAxeMan.GetComponent<UnitController>().Death();
            GameOver();
        }
        

        

    }
    public void AddUnit(GameObject unit)
    {
        //добавляем в отряд
        units.Enqueue(unit.GetComponent<UnitController>());
        
        //логика после того как добавился в отряд
        unit.GetComponent<UnitController>().SetTarget(latestUnit);
        latestUnit = unit;

        UpdateUI();
    }

    public void GameOver()
    {
        unitsCountText.text = "0";
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
    }

    void UpdateUI()
    {
        unitsCountText.text = (units.Count + 1).ToString();
    }

    public void RotateMainDirection(Vector3 direction)
    {
        newDirection = direction;
        
    }

}
