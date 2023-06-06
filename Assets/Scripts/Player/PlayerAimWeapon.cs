using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    private const string WEAPON_BEHIND = "WeaponBehind";
    private const string WEAPON_IN_FRONT = "WeaponInFront";

    private Transform aimTransform;
    private SortingGroup sortingGroup;

    private void Awake() {
        aimTransform = transform.Find("Aim");
        sortingGroup = aimTransform.GetComponent<SortingGroup>();
    }

    private void Update() {
        HandleAiming();
    }

    private void HandleAiming(){
        Vector3 mousePosition = GameInput.Instance.GetMousePosition();
        mousePosition.z = 0f;

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        FixWeaponRotation(angle);
        ChangeWeaponSortingLayer(angle);
    }

    private void FixWeaponRotation(float angle){
        Vector3 localScale = Vector3.one;
        if(angle > 90 || angle < -90){
            localScale.y = -1f;
        } else {
            localScale.y = +1f;
        }
        aimTransform.localScale = localScale;

    }

    private void ChangeWeaponSortingLayer(float angle){
        if(angle > 45 && angle < 135){
            sortingGroup.sortingLayerName = WEAPON_BEHIND;

        } else {
            sortingGroup.sortingLayerName = WEAPON_IN_FRONT;
        }
    }
}
