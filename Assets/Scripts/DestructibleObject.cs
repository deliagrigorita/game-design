using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {
    private Animation hitAnimaton;

    private void Awake() {
        hitAnimaton = transform.Find("Visual").GetComponent<Animation>();
    }

    public void Damage() {
        hitAnimaton.Play("TargetHit");
    }
}

