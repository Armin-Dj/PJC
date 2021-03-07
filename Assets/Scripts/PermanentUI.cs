using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PermanentUI : MonoBehaviour
{
    //Player States
    public int cherries = 0;
    public int hp = 5;
    public TextMeshProUGUI cherryText;
    public TextMeshProUGUI hpAmmount;

    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        cherryText.text = cherries.ToString();

        //singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Reset()
    {
        cherries = 0;
        cherryText.text = cherries.ToString();
    }
}
