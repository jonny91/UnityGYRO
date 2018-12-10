using System;
using UnityEngine;

[Serializable]
public class HierarchyRange
{
    [Tooltip("分层配置")]
    public TransformOriginalPosition[] TransformGroup;

    [Tooltip("X轴移动范围")]
    public float RangeX;

    [Tooltip("Y轴移动范围")]
    public float RangeY;

    [Tooltip("X轴移动时间")]
    public float TimeX;

    [Tooltip("Y轴移动时间")]
    public float TimeY;
}