﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeII;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// READ: the universe. This is the root object for orbital movement.
/// </summary>
public class SolarSystem : MonoBehaviour {
    public static SolarSystem Instance { get; private set; }

    // Number of real units are represented by a game unit
    public float scaleRadius;
    public float scaleOrbits;
    public static float ScaleRadius { get { return Instance.scaleRadius; } }
    public static float ScaleOrbits { get { return Instance.scaleOrbits; } }

    public float timeWarp;
    public static float TimeWarp { get { return Instance.timeWarp; } }
    public GameObject root;

    public RecursiveTree<KeplerTreeNode> tree { get; private set; }

    void Awake() {
        if (Instance != null)
            throw new Exception();

        Instance = this;
        tree = new RecursiveTree<KeplerTreeNode>(root, transform, n => n.satellites);
    }

    public static float GetOrbitDistance(KeplerTreeNode node) {
        var depth = Instance.tree.GetDepth(node);
        return node.distanceFromParent * Mathf.Pow(depth + 1, Instance.scaleOrbits);
    }

    public Orbit MakeOrbit(KeplerTreeNode node) {
        return new Orbit(tree.GetParent(node), node.transform, GetOrbitDistance(node));
    }
}
