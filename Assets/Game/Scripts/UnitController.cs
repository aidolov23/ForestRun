using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    CharacterController playerCharacterController;

    float speed = 4f;

    Animator anim;
    [SerializeField]
    public GameObject target;
    [SerializeField]
    public Vector3 targetMovePosition;

    Vector3 movement;

    public bool isMain = false;

    public GameObject watchZone;

    int status = -1;

    [SerializeField]
    public GameObject deathObj;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float targetDistance = 0;

        if (status == 0 || status == 1)
        {

            if (!target)
            {
                if (targetMovePosition != Vector3.zero)
                {
                    //если еще есть какая-то корректирующая точка без цели
                    movement = (targetMovePosition - transform.position).normalized;

                    float path = (targetMovePosition - transform.position).magnitude;

                    if (path < 1f)
                    {
                        //это была корректирующая точка для движения, сбрасываем ее
                        targetMovePosition = Vector3.zero;
                    }
                }
                else
                {
                    //если нет, то свободное перемещение
                    Vector2 touchDeltaPosition = Vector2.zero;

                    if (isMain)
                    {
                        //для главного идет управление его положением
                        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                        {
                            float touchDistance = Input.GetTouch(0).deltaPosition.x;
                            if(Mathf.Abs(touchDistance) > 3f) //исключение случайных небольших касаний
                            {
                                touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                                #if UNITY_EDITOR
                                touchDeltaPosition *= 25f;
                                #endif
                            }
                            
                        }
                    }

                    movement = PlayerController.instance.transform.forward + PlayerController.instance.transform.right * (touchDeltaPosition.x / 10f);
                }
            }
            else
            {

                //перемещение блокировано на цель
                if (target.CompareTag("Tree"))
                {
                    Debug.Log("Target is tree:" +  target.CompareTag("Tree"));

                    Vector3 treeColliderCenter = target.GetComponent<Collider>().bounds.center;
                    targetMovePosition = new Vector3(treeColliderCenter.x, transform.position.y, treeColliderCenter.z);
                }
                else
                {
                    targetMovePosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
                }
                
                
                
                Debug.DrawRay(targetMovePosition, Vector3.up, Color.red);

                movement = (targetMovePosition - transform.position).normalized;


                targetDistance = (targetMovePosition - transform.position).magnitude;

                float minPath = 0.5f;
                if (!target.CompareTag("Tree"))
                {
                    minPath = 1.75f; //расстояние между дровосеками
                }

                if (targetDistance < minPath)
                {
                    SetStatus(0);
                    movement = movement.normalized * 0.01f; //для того чтобы посмотреть в ту сторону
                    if (target)
                    {
                        //остановка у цели
                        if (target.CompareTag("Tree"))
                        {
                            movement = PlayerController.instance.transform.forward;
                            PlayerController.instance.AtackTarget(target);
                        }
                        
                    }

                }
                else
                {
                    SetStatus(1);
                }
            }

            //движение
            int speedMultiplier = 1;
            if (targetDistance > 4f)
            {
                //если сильно отстает, то ускоряем
                speedMultiplier = 2;
            }

            playerCharacterController.Move(movement * speed * speedMultiplier  * Time.deltaTime);

            if (targetDistance > 10f)
            {
                //если очень сильно отстает, то телепортируем
                transform.position += movement;
            }
        }

        //поворот в сторону движения
        if(movement != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 20f * Time.deltaTime);
        }
        


    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tree") && !target && targetMovePosition == Vector3.zero)
        {
            target = other.gameObject;
            //target.GetComponent<MeshRenderer>().material.color = Color.red;

            Vector3 center = other.bounds.center;
        }
    }

    public void SetStatus(int s)
    {
        status = s;
        anim.SetInteger("status", status);
    }

    public void SetTarget(GameObject g)
    {
        target = g;
        GetComponent<UnitController>().SetStatus(1);
    }

    public void Death()
    {
        Instantiate(deathObj, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
