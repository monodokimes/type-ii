﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    public Transform focus;
    public float initialDistance;
    public float angle;
    
    public float movementSensitivity;

    public float zoomSensitivity;
    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    public bool enablePanning;
    public float panMargin;
    
    private float _distance;
    private float _zoom;

    private Transform _target;
    
    private void Start() {
        _distance = initialDistance;
        _zoom = _distance;
        
        GameObject.FindGameObjectWithTag(GameTags.Input)
            .GetComponent<SelectionManager>()
            .OnSelect
            .AddListener(go => Track(go.transform));
    }
    
    private void Update() {
        var v = GetVerticalAxis();
        var h = GetHorizontalAxis();

        _zoom += Input.GetAxis("Mouse ScrollWheel") * _distance * -zoomSensitivity;
        _zoom = Mathf.Clamp(_zoom, minZoom, maxZoom);

        if (_target != null) {
            focus.transform.position = _target.position;

            if (v != 0 || h != 0) {
                _target = null;
            }
        } else {
            Move(Vector3.forward, v);
            Move(Vector3.right, h);
        }
    }

    private void LateUpdate() {
        if (focus == null)
            return;

        _distance = Mathf.Lerp(_distance, _zoom, Time.deltaTime * zoomSpeed);

        var dir = new Vector3(0, 0, -_distance);
        var rotation = Quaternion.Euler(angle, 0, 0);

        var currentFocus = _target ?? focus;
        transform.position = currentFocus.position + rotation * dir;
        transform.LookAt(currentFocus.position);
    }

    private void Move(Vector3 direction, float axis) {
        focus.transform.Translate(direction * axis * Mathf.Sqrt(_distance) * movementSensitivity * Time.deltaTime);
    }

    public void Track(Transform target) {
        Track(target, _zoom);
    }

    public void Track(Transform target, float distance) {
        _target = target;
        _zoom = distance;
    }

    private float GetVerticalAxis() {
        if (enablePanning) {
            if (Input.mousePosition.y > Screen.height - panMargin) {
                return 1;
            } else if (Input.mousePosition.y < panMargin) {
                return -1;
            }
        }

        return Input.GetAxis("Vertical");
    }

    private float GetHorizontalAxis() {
        if (enablePanning) {
            if (Input.mousePosition.x > Screen.width - panMargin) {
                return 1;
            } else if (Input.mousePosition.x < panMargin) {
                return -1;
            }
        }

        return Input.GetAxis("Horizontal");
    }
}
