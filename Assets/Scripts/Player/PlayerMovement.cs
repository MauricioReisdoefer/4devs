using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float currentSpeed = playerData.speed;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direction = new Vector2(horizontal, vertical);
        direction.Normalize();

        if(horizontal > 0 && vertical > 0)
        {
            direction *= 0.73f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= playerData.runSpeed;

        if (Input.GetKey(KeyCode.LeftControl))
            currentSpeed *= playerData.crouchSpeed;

        rb.AddForce(direction * currentSpeed * Time.deltaTime);
    }
}
