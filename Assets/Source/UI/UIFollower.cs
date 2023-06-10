using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;
    // Update is called once per frame
    void Update()
    {
        if (_target != null)
        {
            transform.position = Camera.main.WorldToScreenPoint(_target.transform.position) + (Vector3)_offset;
        }
    }
}
