using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JoinedTransform : MonoBehaviour
{
    protected abstract void OnTriggerEnter(Collider col);
}
