using System;
using UnityEngine;

public class PlayerInputsService : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

    private Ray _ray;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward);
            var hits = Physics.RaycastAll(_ray, 100, _layerMask);
            
            foreach (var hit in hits)
            {
                var selectableGameObject = hit.transform.gameObject.GetComponent<SelectableGameObject>();
                if (selectableGameObject != null)
                {
                    selectableGameObject.Select();
                    break;
                }
            }
        }
    }
}