using UnityEngine;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    private ClickZoom _clickZoom;

    private void Awake()
    {
        _clickZoom = transform.GetComponent<ClickZoom>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                Hit(objectHit.gameObject);
            }
        }
    }

    private void Hit(GameObject objectHit)
    {
        if (objectHit.CompareTag("Btn"))
        {
            _clickZoom.ZoomIn(objectHit);
        }
        else
        {
            _clickZoom.ZoomOut();
        }
    }
}