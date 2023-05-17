using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public int roomIndex = 0;

    private void OnTriggerEnter2D(Collider2D collider) {
        Player playerObject;
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerObject.SetRoom(roomIndex);
    }

}
