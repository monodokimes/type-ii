﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class SystemView : MonoBehaviour {
    
    private KeplerTreeNode _keplerTree;
    private Rect _panelRect;
    private CameraController _camera;

    void Awake() {
        _panelRect = new Rect(10, 10, 200, Screen.height - 10);
    }

    // Use this for initialization
    void Start() {
        _keplerTree = SolarSystem.tree;
        if (_keplerTree == null)
            throw new Exception();

        _camera = Camera.main.GetComponent<CameraController>();
        Focus(_keplerTree);
    }

    void OnGUI() {
        GUI.Box(_panelRect, "Solar System");

        var buttonPosition = new Vector2(_panelRect.x + 10, _panelRect.y + 30);
        Node((int)buttonPosition.x, (int)buttonPosition.y, _keplerTree);
    }

    private int Node(int x, int y, KeplerTreeNode data, int recursionLevel = 0) {
        var buttonWidth = 100;
        var buttonHeight = 20;
        var buttonMargin = 5;
        var indentMargin = 10;

        if (GUI.Button(new Rect(x + recursionLevel * indentMargin, y, buttonWidth, buttonHeight), data.designation)) {
            Focus(data);
            Debug.Log("");
        }

        y += buttonHeight + buttonMargin;
        foreach (var datum in data.Satellites()) {
            y = Node(x, y, datum, recursionLevel + 1);
        }

        return y;
    }

    private void Focus(KeplerTreeNode node) {
        var focusDistance = 2;

        _camera.Track(node.transform, focusDistance * node.radius * SolarSystem.ScaleRadius);
    }
}
