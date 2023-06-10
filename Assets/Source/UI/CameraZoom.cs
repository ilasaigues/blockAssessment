using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    [SerializeField] Vector3 _farPos;
    [SerializeField] Vector3 _nearPos;
    [SerializeField] float _zoomSpeed;

    private float _lerp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _lerp += Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * _zoomSpeed;
        _lerp = Mathf.Clamp01(_lerp);
        transform.localPosition = Vector3.Lerp(_farPos, _nearPos, _lerp);
    }
}
