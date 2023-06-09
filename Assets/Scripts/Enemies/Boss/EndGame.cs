using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    private float endTime = 500f;

    void Start()
    {
        Effects.Instance.PlayEndGame(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        endTime -= 1;

        if(endTime < 0){
            SceneManager.LoadScene("WinGame");
        }
        
    }
}
