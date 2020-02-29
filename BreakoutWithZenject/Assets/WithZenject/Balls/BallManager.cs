using Zenject;
using UnityEngine;
using System.Collections.Generic;

namespace WithZenject
{
    public class BallManager
    {
        readonly List<Ball> _ballPool = new List<Ball>();
        readonly Ball.Factory _ballFactory;
        readonly Settings _settings;

        public BallManager(Settings settings, Ball.Factory factory)
        {
            _settings = settings;
            _ballFactory = factory;
        }

        public void SpawnNewBall()
        {
            if (_ballPool.Count < _settings.MaxSpawnAllowed)
            {
                _ballPool.Add(_ballFactory.Create());
            }

        }

        [System.Serializable]
        public class Settings
        {
            public int MaxSpawnAllowed = 0;
        }
    }
}