using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndShowCase : MonoBehaviour
{
    [SerializeField] private GameObject finalScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            finalScreen.SetActive(true);
        }
    }
}
