using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Animation hitAnimaton;

    private void Awake() {
        hitAnimaton = transform.Find("TargetVisual").GetComponent<Animation>();
    }

    public void Damage(){
        hitAnimaton.Play("TargetHit");
    }
}
