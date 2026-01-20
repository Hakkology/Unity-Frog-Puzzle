using UnityEngine;

/// <summary>
/// Handles player input via raycasting to interact with grid objects.
/// </summary>
public class PlayerRaycaster : MonoBehaviour
{
    Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                BaseObject baseObject = hit.collider.GetComponent<BaseObject>();
                if (baseObject != null)
                    baseObject.Interact();
                
                else
                {
                    Debug.Log("Hit object does not have BaseObject component.");
                }
            }
            else
            {
                Debug.Log("Raycast did not hit any objects.");
            }
        }
    }
}
