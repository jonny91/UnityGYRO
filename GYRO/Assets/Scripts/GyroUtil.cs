using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GyroUtil : MonoBehaviour
{
    [SerializeField] private Transform _rotationTransform;
    [SerializeField] private Transform[] _top;
    [SerializeField] private Text _logText;

    private List<TransformOriginalPosition> _positions;

    private const float FactorY = 1.5f;
    private const float FactorX = 1f;

    private const float BackGroundMoveRange = 0.2f;
    private const float BackGroundMoveTime = 6f;

    private void Awake()
    {
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true
        Input.gyro.enabled = true;
        //获取设备重力加速度向量
        Vector3 deviceGravity = Input.gyro.gravity;
        //设备的旋转速度，返回结果为x，y，z轴的旋转速度，单位为（弧度/秒)
        Vector3 rotationVelocity = Input.gyro.rotationRate;
        //获取更加精确的旋转
        Vector3 rotationVelocity2 = Input.gyro.rotationRateUnbiased;
        //设置陀螺仪的更新检索时间，即隔 0.1秒更新一次
        Input.gyro.updateInterval = 0.1f;
        //获取移除重力加速度后设备的加速度
        Vector3 acceleration = Input.gyro.userAcceleration;


        _positions = new List<TransformOriginalPosition>();
        foreach (var t in _top)
        {
            var p = new TransformOriginalPosition {Transform = t};
            p.OriginalPosition = p.Transform.localPosition;

            _positions.Add(p);
        }
    }

    protected void Update()
    {
        if (SystemInfo.supportsGyroscope && Input.gyro.attitude != Quaternion.identity)
        {
            var eulerAngles = Input.gyro.attitude.eulerAngles;
            _logText.text = string.Format("x={0}\ny={1}\nz={2}\n",
                eulerAngles.x,
                eulerAngles.y,
                eulerAngles.z);

            float rad = 0f;
            for (var index = 0; index < _positions.Count; index++)
            {
                var p = _positions[index];
                //纵向 往上 0-20
                if (eulerAngles.y > 0 && eulerAngles.y <= 20)
                {
                    rad = eulerAngles.y * Mathf.Deg2Rad;
                    p.Transform.DOLocalMoveY(p.OriginalPosition.y + rad * FactorY * (1 + index * 0.2f), 1f)
                        .SetEase(Ease.OutBack)
                        .SetAutoKill(true);

                    _rotationTransform.DOLocalMoveY(-BackGroundMoveRange, BackGroundMoveTime).SetEase(Ease.OutBack)
                        .SetAutoKill(true);
                }
                else if (eulerAngles.y >= 340 && eulerAngles.y < 360) //往下 360-340
                {
                    rad = eulerAngles.y * Mathf.Deg2Rad - 2 * Mathf.PI;
                    p.Transform.DOLocalMoveY(p.OriginalPosition.y + rad * FactorY * (1 + index * 0.2f), 1f)
                        .SetEase(Ease.OutBack)
                        .SetAutoKill(true);

                    _rotationTransform.DOLocalMoveY(BackGroundMoveRange, BackGroundMoveTime).SetEase(Ease.OutBack)
                        .SetAutoKill(true);
                }


                //横向 往左边 360-340
                if (eulerAngles.x >= 340 && eulerAngles.x < 360)
                {
                    rad = eulerAngles.x * Mathf.Deg2Rad - 2 * Mathf.PI;
                    p.Transform.DOLocalMoveX(p.OriginalPosition.x + rad * FactorX, 1f).SetAutoKill(true);

                    _rotationTransform.DOLocalMoveX(BackGroundMoveRange, BackGroundMoveTime).SetEase(Ease.OutBack)
                        .SetAutoKill(true);
                }
                else if (eulerAngles.x > 0 && eulerAngles.x <= 20) //往右边 0-20
                {
                    rad = eulerAngles.x * Mathf.Deg2Rad;
                    p.Transform.DOLocalMoveX(p.OriginalPosition.x + rad * FactorX, 1f).SetAutoKill(true);

                    _rotationTransform.DOLocalMoveX(-BackGroundMoveRange, BackGroundMoveTime).SetEase(Ease.OutBack)
                        .SetAutoKill(true);
                }
            }
        }
    }
}