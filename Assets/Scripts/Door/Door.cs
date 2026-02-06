using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle;

    [SerializeField] private float speed = 2f;
    private bool isOpen = false;

    private Quaternion closedQuaternion;
    private Quaternion openQuaternion;

    private bool playerIsClose = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            playerIsClose = false;
        }
    }

    void Start()
    {
        
        closedQuaternion = transform.rotation;
        openQuaternion = Quaternion.Euler(0, 0, openAngle) * closedQuaternion;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            isOpen = !isOpen;
        }
        transform.rotation = Quaternion.Lerp(
            transform.rotation, isOpen ? openQuaternion : closedQuaternion, speed * Time.deltaTime);
    }
}
