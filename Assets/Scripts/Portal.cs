using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public int roomIndex = 0;

     private void OnTriggerEnter2D(Collider2D collider) {
        Player playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerObject.SetRoom(roomIndex);
        if(roomIndex == int.MaxValue / 2) {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().ActivateBar(true);
        } else {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().ActivateBar(false);
        }
    }

}
