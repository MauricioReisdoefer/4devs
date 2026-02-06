using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "playerData",menuName = "Data/Player")]
public class PlayerData : ScriptableObject
{
    public float speed;
    public float runSpeed;
    public float crouchSpeed;

    public float damage;
    public float reloadTime;
}
