﻿using UnityEngine;

public class Brick : MonoBehaviour {

    private void OnCollisionEnter2D (Collision2D other) {
        gameObject.SetActive (false);
    }
}