
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;
    // Start is called before the first frame update
    void Awake()
    {
        _target = GameObject.FindWithTag("CameraPoint").transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            transform.position = _target.position;
            transform.rotation = _target.rotation;
        }
        
    }
}
