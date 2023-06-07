using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    private float endTime = 10f;

    void Start()
    {
        Effects.Instance.PlayEndGame(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        endTime -= 1;

        if(endTime < 0){
            // to do
        }
        
    }
}
