using Game;
using UnityEngine;

namespace Services
{
    public class PlayerInputsService : MonoBehaviour
    {
        public bool Enabled;
    
        [SerializeField] private LayerMask _layerMask;

        private Ray _ray;


        public void EnableInputs() => Enabled = true;
        public void DisableInputs() => Enabled = false;

        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            Enabled = true;
        }
    
        private void Update()
        {
            if (!Enabled)
                return;
        
            if (Input.GetMouseButton(0))
            {
                var hits = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, _layerMask);
            
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
}