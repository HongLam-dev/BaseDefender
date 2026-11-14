using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 offsetPosition;
    [SerializeField] private Vector3 offsetRotaton;
    [SerializeField] float followSpeed=5;
    [SerializeField] float lerpDistance = 0.1f;

    private Camera followCamera;
    void Start()
    {
        followCamera = GetComponent<Camera>();

        transform.eulerAngles = offsetRotaton;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPostion = target.transform.position + offsetPosition;
        if (Vector3.Distance(transform.position-offsetPosition,target.position)>lerpDistance)
        {
            targetPostion = Vector3.Lerp(
            transform.position,
            targetPostion,
            followSpeed * Time.deltaTime
            );
        }
        transform.position = targetPostion;

    }
}
