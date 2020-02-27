using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {

    [SerializeField] private float m_Speed;
    [SerializeField][Range (0.1f, 1)] private float m_MinSpeedMultipler;
    private Rigidbody2D _ballBody;

    public static Action OnBallCollision;

    void Start () {
        _ballBody = GetComponent<Rigidbody2D> ();

        Vector2 randomVelocity = Random.insideUnitCircle;
        randomVelocity.x = Mathf.Clamp (randomVelocity.x, m_MinSpeedMultipler, 1);
        randomVelocity.y = Mathf.Clamp (randomVelocity.y, m_MinSpeedMultipler, 1);

        _ballBody.velocity = randomVelocity * m_Speed;
    }

    private void OnCollisionEnter2D (Collision2D other) {
        OnBallCollision?.Invoke ();
    }
}