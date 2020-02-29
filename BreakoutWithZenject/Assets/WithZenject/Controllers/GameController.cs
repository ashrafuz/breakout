using UnityEngine;
using Zenject;
using ModestTree;

namespace WithZenject
{

    public class GameController : IInitializable, ITickable, ILateDisposable
    {

        private SignalBus _signalBus;
        private BallManager _ballManager;

        private Setting _setting;

        private int _playerScore;
        private bool _hasStarted;

        public GameController(Setting setting, SignalBus signal, BallManager ballManager)
        {
            _signalBus = signal;
            _setting = setting;
            _ballManager = ballManager;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BallCollidedSignal>(OnBallCollision);
        }

        private void OnBallCollision(BallCollidedSignal ballCollision)
        {
            GameObject collidedWith = ballCollision.CollidedWith;
            //Assumption: Side wall collisions are avoided, only paddle & brick collision will cause to score
            if (collidedWith.GetComponent<Paddle>() != null || collidedWith.GetComponent<Brick>() != null)
            {
                _playerScore += Random.Range(_setting.MinRandomPoint, _setting.MaxRandomPoint);
                _signalBus.Fire(new NewScoreUpdateSignal() { NewScore = _playerScore });
            }
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _hasStarted = true;
                _ballManager.SpawnNewBall();
                _signalBus.Fire<GameStartedSignal>();
            }

            if (Input.GetMouseButtonDown(0) && _hasStarted)
            {
                _ballManager.SpawnNewBall();
            }
        }

        public void LateDispose()
        {
            _signalBus.Unsubscribe<BallCollidedSignal>(OnBallCollision);
        }

        [System.Serializable]
        public class Setting
        {
            public int MinRandomPoint;
            public int MaxRandomPoint;
        }
    }
}