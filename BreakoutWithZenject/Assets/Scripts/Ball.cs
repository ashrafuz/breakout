using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour {
    public Action<GameObject> OnBallCollision; //subscription channel
    [Range (5, 30)] public float Speed;

    [Range (0.01f, 0.2f)] private float m_RandomMoveFactor = 0.1f;
    private Rigidbody2D _ballBody;

    private void Awake () {
        _ballBody = GetComponent<Rigidbody2D> ();
    }

    public void StartMoving (Vector2 direction) {
        _ballBody.velocity = direction.normalized * Speed;
    }

    private void OnCollisionExit2D (Collision2D other) {
        OnBallCollision?.Invoke (other.gameObject);

        //to prevent looping movements
        Vector2 newVelocity = _ballBody.velocity.normalized;
        newVelocity.x += Random.Range (-m_RandomMoveFactor, m_RandomMoveFactor);
        newVelocity.y += Random.Range (-m_RandomMoveFactor, m_RandomMoveFactor);

        _ballBody.velocity = newVelocity.normalized * Speed;
    }
}