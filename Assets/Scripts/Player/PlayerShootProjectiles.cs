using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectiles : MonoBehaviour
{
    private Transform aimTransform;
    private Transform aimVisuals;
    private Transform aimGunEndPointTransform;
    private Animator aimAnimator;
    private Vector3 gunEndPointPosition;
    private Vector3 shootPosition;

    [SerializeField] private Transform pfBullet;

    private void Awake() {
        aimTransform = transform.Find("Aim");
        aimVisuals = aimTransform.Find("Visuals");
        aimGunEndPointTransform = aimVisuals.Find("GunEndPointPosition");
        aimAnimator = aimTransform.GetComponent<Animator>();
    }

    private void Start() {
        GameInput.Instance.OnMouseClicked += PlayerShoot;
    }

    private void PlayerShoot(object sender, EventArgs e){

        gunEndPointPosition = aimGunEndPointTransform.position;
        shootPosition = GameInput.Instance.GetMousePosition();
        shootPosition.z = 0f;

        Transform bulletTransform = Instantiate(pfBullet, gunEndPointPosition, Quaternion.identity);

        Vector3 shootDir = (shootPosition - gunEndPointPosition).normalized;
        // shootDir = Vector3.ClampMagnitude(shootDir, 1);
        bulletTransform.GetComponent<Bullet>().Setup(shootDir);


        CinemachineShake.Instance.ShakeCamera(3f, 0.25f);
        aimAnimator.SetTrigger("Shoot");

    }
}
