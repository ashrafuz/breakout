using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour {

    private BrickSpawner _brickSpawner;

    public void SetSpawner (BrickSpawner spawner) {
        _brickSpawner = spawner;
    }

    private void OnCollisionEnter2D (Collision2D other) {
        _brickSpawner.RemoveSelf (this);
    }
}