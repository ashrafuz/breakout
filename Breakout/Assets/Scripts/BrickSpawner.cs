using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour {
    [SerializeField][Range (0.1f, 2)] private float m_Margin;
    [SerializeField][Range (0.1f, 2)] private float m_Padding;

    [SerializeField][Range (2, 20)] private float m_SpawnFrequecyInSeconds;
    [SerializeField] private Brick m_BrickPrefab;

    private Boundary _boundary;
    private float _elapsedTime = 0;

    private List<Brick> _idleBrickPool = new List<Brick> ();
    private List<Vector2> _idlePositionPool = new List<Vector2> ();

    private GameManager _gameManager;

    void Start () {
        InitalizeBoundary ();
        InitializeSpawnPositions ();
        GameManager.OnGameStart += OnGameStart;
    }

    private void InitializeSpawnPositions () {
        Vector2 brickSize = m_BrickPrefab.GetComponent<SpriteRenderer> ().bounds.size;
        //Assumption: Only Top half is allowed for spawning
        Vector2 startPos = new Vector2 (0, 0);

        while ((startPos.y + brickSize.y * 0.5f) < _boundary.Top) {
            _idlePositionPool.Add (startPos);
            _idlePositionPool.Add (new Vector2 (-startPos.x, startPos.y));

            startPos.x += brickSize.x + m_Padding;
            if ((startPos.x + brickSize.x * 0.5f) > _boundary.Right) {
                startPos.x = 0;
                startPos.y += brickSize.y + m_Padding;
            }
        }
    }

    private void InitalizeBoundary () {
        //Assumption: Horizontal & Vertical margin is same
        Camera mc = Camera.main;
        _boundary = new Boundary (mc, m_Margin);
    }

    private void OnGameStart (GameManager gm) {
        _gameManager = gm;
        SpawnBrick ();
    }

    private void Update () {
        if (_gameManager == null) { return; }

        if (_gameManager.IsRunning && _idlePositionPool.Count > 0) {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= m_SpawnFrequecyInSeconds) {
                _elapsedTime = 0;
                SpawnBrick ();
            }
        }
    }

    private Brick SpawnBrick () {
        Vector2 pos = _idlePositionPool[Random.Range (0, _idlePositionPool.Count)];
        _idlePositionPool.Remove (pos);

        Brick brickToSpawn = null;
        if (_idleBrickPool.Count > 0) {
            brickToSpawn = _idleBrickPool[0];
            _idleBrickPool.Remove (brickToSpawn);
        } else {
            brickToSpawn = Instantiate (m_BrickPrefab, this.transform);
            brickToSpawn.SetSpawner (this);
        }

        brickToSpawn.transform.position = pos;
        brickToSpawn.gameObject.SetActive (true);
        return brickToSpawn;
    }

    public void RemoveSelf (Brick brick) {
        if (!_idleBrickPool.Contains (brick)) { //just for safety
            _idleBrickPool.Add (brick);
        }

        if (!_idlePositionPool.Contains (brick.transform.position)) { //just for safety
            _idlePositionPool.Add (brick.transform.position);
        }

        brick.gameObject.SetActive (false);
    }
}