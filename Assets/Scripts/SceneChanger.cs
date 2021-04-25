using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    public bool isStart;
    public bool isQuit;
    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    

}
