using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLantern : MonoBehaviour
{
    [SerializeField] private float distanceBehind = 1.2f;
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        worldMousePos.z = 0f;

        Transform player = transform.parent;

        Vector2 direction = (worldMousePos - player.position).normalized;

        Vector2 localOffset = -direction * distanceBehind;
        transform.localPosition = localOffset;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}