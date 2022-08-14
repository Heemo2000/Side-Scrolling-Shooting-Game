using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    [SerializeField]private Transform target;

    public Transform Target { get => target; set => target = value; }

    protected abstract void HandleBehaviour();
}
