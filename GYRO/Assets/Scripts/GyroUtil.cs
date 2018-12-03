using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

internal class TransformOrginalPosition
{
    public Transform Transform;
    public Vector3 OrginalPosition;
}

public class GyroUtil : MonoBehaviour
{
    [SerializeField] private Transform _rotationTransform;
    [SerializeField] private Transform[] _top;
    [SerializeField] private Text _logText;

    private float _range = 1f;

    private List<TransformOrginalPosition> _positions;

    private const float LowPassFilterFactor = 5f;

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


        _positions = new List<TransformOrginalPosition>();
        foreach (var t in _top)
        {
            var p = new TransformOrginalPosition {Transform = t};
            p.OrginalPosition = p.Transform.localPosition;

            _positions.Add(p);
        }
    }

    protected void Update()
    {
        if (SystemInfo.supportsGyroscope && Input.gyro.attitude != Quaternion.identity)
        {
//            _rotationTransform.rotation =
//                Quaternion.Slerp(_rotationTransform.rotation, Input.gyro.attitude,
//                    LowPassFilterFactor * Time.deltaTime);

            _logText.text = string.Format("Input.gyro.rotationRate\nx:{0}\ny:{1}\nz:{2}",
                Input.gyro.rotationRate.x,
                Input.gyro.rotationRate.y,
                Input.gyro.rotationRate.z);

            var xSpeed = Input.gyro.rotationRate.x;
            var ySpeed = Input.gyro.rotationRate.y;

            foreach (var p in _positions)
            {
                if (Mathf.Abs(xSpeed) > 0.1f)
                {
                    p.Transform.localPosition = new Vector3(
                        p.OrginalPosition.x,
                        Mathf.Lerp(p.Transform.localPosition.y,
                            Mathf.Clamp(
                                p.OrginalPosition.y + xSpeed * LowPassFilterFactor * Time.deltaTime,
                                p.OrginalPosition.y - _range,
                                p.OrginalPosition.y + _range
                            ), 0.1f),
                        p.OrginalPosition.z);
                }
                else
                {
                    //缓动到初始位置
                    p.Transform.localPosition =
                        Vector3.Lerp(p.Transform.localPosition, new Vector3(
                            p.Transform.localPosition.x,
                            p.OrginalPosition.y,
                            p.Transform.localPosition.z
                        ), 0.1f);
                }


                if (Mathf.Abs(ySpeed) > 0.1f)
                {
                    p.Transform.localPosition = new Vector3(
                        Mathf.Lerp(p.Transform.localPosition.x,
                            Mathf.Clamp(
                                p.OrginalPosition.x + ySpeed * LowPassFilterFactor * Time.deltaTime,
                                p.OrginalPosition.x - _range,
                                p.OrginalPosition.x + _range
                            ), 0.1f),
                        p.OrginalPosition.y,
                        p.OrginalPosition.z);
                }
                else
                {
                    //缓动到初始位置
                    p.Transform.localPosition =
                        Vector3.Lerp(p.Transform.localPosition, new Vector3(
                            p.OrginalPosition.x,
                            p.Transform.localPosition.y,
                            p.Transform.localPosition.z
                        ), 0.1f);
                }
            }
        }
    }
}