using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeController : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Text liveText;

    [SerializeField]
    GameObject trunk;
    [SerializeField]
    GameObject head;

    [SerializeField]
    public GameObject deathObj;

    int lives;
    // Start is called before the first frame update
    void Start()
    {
        canvas.GetComponent<Canvas>().worldCamera = Camera.main;

        lives = Random.Range(1, 4);

        trunk.transform.localScale += new Vector3(0, trunk.transform.localScale.y * (lives - 1), 0);
        trunk.transform.position = new Vector3(trunk.transform.position.x, trunk.transform.lossyScale.y, trunk.transform.position.z);

        canvas.transform.position = new Vector3(canvas.transform.position.x, trunk.transform.position.y * 2f, canvas.transform.position.z);

        head.transform.position = new Vector3(head.transform.position.x, canvas.transform.position.y, head.transform.position.z);

        UpdateUI();
    }

    public int GetDamage()
    {
        lives--;
        UpdateUI();

        if(lives == 0)
        {
            Death();
        }

        return lives;
    }

    void UpdateUI()
    {
        liveText.text = lives.ToString();
    }

    void Death()
    {
        Instantiate(deathObj, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
