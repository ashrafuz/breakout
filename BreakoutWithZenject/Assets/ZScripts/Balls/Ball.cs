using UnityEngine;
using Zenject;

namespace WithZenject {

    public class Ball : MonoBehaviour {
        private SignalBus _signalBus;
        private Settings _settings;
        private Rigidbody2D _rigidBody;

        [Inject]
        public void Construct (Settings setting, SignalBus signal) {
            _settings = setting;
            _signalBus = signal;
            _rigidBody = GetComponent<Rigidbody2D> ();
        }

        private void Start () {
            _rigidBody.velocity = GetRandomStartDirection ().normalized * _settings.Speed;
        }

        private Vector2 GetRandomStartDirection () {
            return new Vector2 (Random.Range (-1, 1), _settings.Speed);
        }

        private void OnCollisionExit2D (Collision2D other) {
            //there might be more optimized way to pass this signal
            _signalBus.Fire (new BallCollidedSignal () { CollidedWith = other.gameObject });

            //to prevent looping movements
            Vector2 newVelocity = _rigidBody.velocity.normalized;
            newVelocity.x += Random.Range (-_settings.RandomMoveFactor, _settings.RandomMoveFactor);
            newVelocity.y += Random.Range (-_settings.RandomMoveFactor, _settings.RandomMoveFactor);

            _rigidBody.velocity = newVelocity.normalized * _settings.Speed;
        }

        //class helpers
        [System.Serializable]
        public class Settings {
            public float Speed;
            [Range (0.01f, 0.2f)] public float RandomMoveFactor;
        }

        public class Factory : PlaceholderFactory<Ball> {

        }
    }

}