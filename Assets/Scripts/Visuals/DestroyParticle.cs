using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    [SerializeField] float lifeTime;

    void Start()
    {
        Destroy(this.gameObject, lifeTime);
        
    }

}
