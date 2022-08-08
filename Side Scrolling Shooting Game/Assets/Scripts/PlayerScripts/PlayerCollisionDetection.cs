using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetection : MonoBehaviour
{
    [SerializeField]private Player player;

    public Player GetPlayerReference()
    {
        return player;
    }
}
