using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotaton;

    private Camera followCamera;
    void Start()
    {
        followCamera = GetComponent<Camera>();

        transform.eulerAngles = offsetRotaton;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + offsetPosition;
    }
}
