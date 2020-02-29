using UnityEngine;
using Zenject;

namespace WithZenject
{
    public class Paddle : MonoBehaviour, ILateDisposable
    {
        private Camera _mainCam;
        private Boundary _boundary;
        private float _currentMousePosX = 0;

        private bool _hasStarted = false;
        private SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signal)
        {
            _signalBus = signal;
        }

        void Start()
        {
            _mainCam = Camera.main;

            float paddleWidth = GetComponent<SpriteRenderer>().bounds.size.x;
            _boundary = new Boundary(_mainCam, paddleWidth * 0.75f);

            _signalBus.Subscribe<GameStartedSignal>(OnGameStart);
        }

        void OnGameStart()
        {
            _hasStarted = true;
        }

        private void Update()
        {
            if (_hasStarted)
            {
                _currentMousePosX = _mainCam.ScreenToWorldPoint(Input.mousePosition).x;
                _currentMousePosX = Mathf.Clamp(_currentMousePosX, _boundary.Left, _boundary.Right);
            }
        }

        void FixedUpdate()
        {
            transform.position = new Vector2(_currentMousePosX, transform.position.y);
        }

        public void LateDispose()
        {
            _signalBus.Unsubscribe<GameStartedSignal>(OnGameStart);
        }
    }

}