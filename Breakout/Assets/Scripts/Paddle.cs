using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour {
    Camera _mainCam;

    [SerializeField][Range (0, 2)] private float m_PaddingFromScreen;

    private float _leftBoundary = 0;
    private float _rightBoundary = 0;
    private float _paddleWidth = 0;

    [SerializeField] float currentMousePosX = 0;
    void Start () {
        _mainCam = Camera.main;
        _paddleWidth = GetComponent<SpriteRenderer> ().bounds.size.x;

        float totalPadding = _paddleWidth * 0.5f + m_PaddingFromScreen;
        _leftBoundary = GameUtil.GetLeftBoundary (_mainCam) + totalPadding;
        _rightBoundary = GameUtil.GetRightBoundary (_mainCam) - totalPadding;
    }

    private void Update () {
        currentMousePosX = _mainCam.ScreenToWorldPoint (Input.mousePosition).x;
        currentMousePosX = Mathf.Clamp (currentMousePosX, _leftBoundary, _rightBoundary);
    }

    void FixedUpdate () {
        transform.position = new Vector2 (currentMousePosX, transform.position.y);
    }
}