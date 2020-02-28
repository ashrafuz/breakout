using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    [SerializeField] private Ball m_BallPrefab;
    [SerializeField] private int m_MaxBallAllowed;
    private List<Ball> _ballPool = new List<Ball> (); //used list so that we can pool later if necessary

    private int _playerScore;
    public int Score => _playerScore;

    private bool _isRunning;
    public bool IsRunning => _isRunning;

    public static Action<GameManager> OnGameStart; //subscription channel

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            if (!_isRunning) {
                _isRunning = true;
                SpawnNewBall ();
                OnGameStart?.Invoke (this);
            }
        }

        if (IsRunning && Input.GetMouseButtonDown (0)) {
            SpawnNewBall ();
        }
    }

    void SpawnNewBall () {
        if (_ballPool.Count < m_MaxBallAllowed) {
            Ball newBall = Instantiate (m_BallPrefab);
            _ballPool.Add (newBall);

            newBall.OnBallCollision += OnCollisionOfBall;

            Vector2 dir = new Vector2 (Random.Range (-1, 1), newBall.Speed);
            newBall.transform.position = Vector2.zero;
            newBall.StartMoving (dir);
        } else {
            Debug.LogWarning ("Max limit reached!");
        }
    }

    private void OnCollisionOfBall (GameObject collidedWith) {
        if (!_isRunning) {
            return;
        }

        //Assumption: Only brick collision adds point.
        if (collidedWith.GetComponent<Brick> () != null) {
            _playerScore += Random.Range (5, 20);
        }
    }

}