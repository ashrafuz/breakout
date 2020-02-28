﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Paddle : MonoBehaviour {
    private Camera _mainCam;
    private float _paddleWidth = 0;
    private float _currentMousePosX = 0;
    private Boundary _boundary;
    private GameManager _gameManager;

    void Start () {
        _mainCam = Camera.main;
        _paddleWidth = GetComponent<SpriteRenderer> ().bounds.size.x;
        _boundary = new Boundary (_mainCam, _paddleWidth * 0.75f);

        GameManager.OnGameStart += OnGameStart;
    }

    private void Update () {
        if (_gameManager == null) {
            return;
        }

        _currentMousePosX = _mainCam.ScreenToWorldPoint (Input.mousePosition).x;
        _currentMousePosX = Mathf.Clamp (_currentMousePosX, _boundary.Left, _boundary.Right);
    }

    void FixedUpdate () {
        if (_gameManager == null) {
            return;
        }
        if (_gameManager.IsRunning) {
            transform.position = new Vector2 (_currentMousePosX, transform.position.y);
        }
    }

    private void OnGameStart (GameManager gm) {
        _gameManager = gm;
    }
}