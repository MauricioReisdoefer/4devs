using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject showE;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            showE.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            showE.SetActive(false);
        }
    }
}
