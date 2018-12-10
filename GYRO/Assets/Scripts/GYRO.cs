using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GYRO : MonoBehaviour
{
    public HierarchyRange[] HierarchyRanges;

    [SerializeField]
    private Text _logText;

    private void Awake()
    {
        //设置设备陀螺仪的开启/关闭状态，使用陀螺仪功能必须设置为 true
        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.1f;

        foreach (var hierarchyRange in HierarchyRanges)
        {
            foreach (var originalPosition in hierarchyRange.TransformGroup)
            {
                originalPosition.OriginalPosition = originalPosition.Transform.localPosition;
            }
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

            foreach (var hierarchyRange in HierarchyRanges)
            {
                foreach (var t in hierarchyRange.TransformGroup)
                {
                    if (eulerAngles.y > 0 && eulerAngles.y <= 20)
                    {
                        rad = eulerAngles.y * Mathf.Deg2Rad;
                    }
                    else if (eulerAngles.y >= 340 && eulerAngles.y < 360) //往下 360-340
                    {
                        rad = eulerAngles.y * Mathf.Deg2Rad - 2 * Mathf.PI;
                    }

                    t.Transform.DOLocalMoveY(t.OriginalPosition.y + rad * hierarchyRange.RangeY,
                        hierarchyRange.TimeY).SetEase(Ease.OutBack).SetAutoKill(true);


                    //横向 往左边 360-340
                    if (eulerAngles.x >= 340 && eulerAngles.x < 360)
                    {
                        rad = eulerAngles.x * Mathf.Deg2Rad - 2 * Mathf.PI;
                    }
                    else if (eulerAngles.x > 0 && eulerAngles.x <= 20) //往右边 0-20
                    {
                        rad = eulerAngles.x * Mathf.Deg2Rad;
                    }

                    t.Transform.DOLocalMoveX(t.OriginalPosition.x + rad * hierarchyRange.RangeX,
                        hierarchyRange.TimeX).SetAutoKill(true);
                }
            }
        }
    }
}