using DG.Tweening;
using UnityEngine;

public class ClickZoom : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Camera _zoomCamera;

    private Tweener _zoomInTween;
    private Tweener _zoomOutTween;

    private Tween MoveCameraIn(GameObject target)
    {
        var targetPosition = target.transform.localPosition;
        targetPosition.x += 1f;
        targetPosition.z = _zoomCamera.transform.localPosition.z;
        return _zoomCamera.transform.DOLocalMove(targetPosition, 0.4f).SetEase(Ease.Linear);
    }

    public void ZoomIn(GameObject target)
    {
        _mainCamera.enabled = false;
        _zoomCamera.enabled = true;

        _zoomInTween = _zoomCamera.DOOrthoSize(2, 0.5f)
            .SetEase(Ease.Linear).SetAutoKill(true);

        MoveCameraIn(target);
    }


    private Tween MoveCameraOut()
    {
        return _zoomCamera.transform.DOLocalMove(new Vector3(0, 0, -10), 0.5f).SetEase(Ease.InFlash);
    }


    public void ZoomOut()
    {
        KillAllTween();
        MoveCameraOut();
        _zoomOutTween = _zoomCamera.DOOrthoSize(5, 1).OnComplete(() =>
        {
            _mainCamera.enabled = true;
            _zoomCamera.enabled = false;
        }).SetEase(Ease.InFlash).SetAutoKill(true);
    }

    private void KillAllTween()
    {
        if (_zoomInTween != null)
        {
            _zoomInTween.Kill();
            _zoomInTween = null;
        }

        if (_zoomOutTween != null)
        {
            _zoomOutTween.Kill();
            _zoomOutTween = null;
        }
    }
}