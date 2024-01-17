using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMe : MonoBehaviour, ILookAtMe
{
    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }
}
