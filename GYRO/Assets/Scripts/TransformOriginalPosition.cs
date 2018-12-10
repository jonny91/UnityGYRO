using System;
using UnityEngine;

[Serializable]
public class TransformOriginalPosition
{
    public Transform Transform;
    [HideInInspector]
    public Vector3 OriginalPosition;
}