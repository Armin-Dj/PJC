using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Fall : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {

        if(coll.gameObject.tag == "Player")
        {
            PermanentUI.perm.Reset();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
