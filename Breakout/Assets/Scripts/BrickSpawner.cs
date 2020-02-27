using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour {

    [SerializeField][Range (0, 2)] private float m_PaddingFromScreen;
    [SerializeField][Range (2, 20)] private float m_SpawnFrequency;

    [SerializeField] private Brick m_BrickPrefab;

    private Boundary _boundary;
    private float _elapsedTime = 0;
    private List<Brick> _brickPool = new List<Brick> ();

    void Start () {
        _boundary = new Boundary (Camera.main, m_PaddingFromScreen);
        SpawnNewBrick ();
    }

    private void Update () {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime >= m_SpawnFrequency) {
            _elapsedTime = 0;
            SpawnNewBrick ();
        }
    }

    private void SpawnNewBrick () {
        Vector2 pos = GetRandomPosition ();
        Brick brickToSpawn = null;

        //looking for idle brick in pool
        for (int i = 0; i < _brickPool.Count; i++) {
            if (!_brickPool[i].gameObject.activeInHierarchy) {
                brickToSpawn = _brickPool[i];
                break;
            }
        }

        if (brickToSpawn == null) { //if no idle brick found, instantiate new
            brickToSpawn = Instantiate (m_BrickPrefab, this.transform);

            _brickPool.Add (brickToSpawn);
        }
        brickToSpawn.transform.position = pos;
        brickToSpawn.gameObject.SetActive (true);
    }

    private Vector2 GetRandomPosition () {
        float x = Random.Range (_boundary.Left, _boundary.Right);
        //assuming that spawned box will only spawn at top half of the screen
        float y = Random.Range (_boundary.Top * 0.5f, _boundary.Top);
        return new Vector2 (x, y);
    }
}