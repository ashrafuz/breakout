using UnityEngine;
using Zenject;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

namespace WithZenject
{
    public class BrickManager : ITickable, ILateDisposable
    {

        readonly List<Brick> _brickPool = new List<Brick>();
        readonly Brick.Factory _brickFactory;
        readonly Settings _settings;
        readonly SignalBus _signal;

        private Boundary _boundary;
        private float _elapsedTime = 0;
        private bool _hasStarted = false;

        public BrickManager(Settings settings, SignalBus signal, Brick.Factory brickFactory)
        {
            _settings = settings;
            _signal = signal;
            _brickFactory = brickFactory;
            _boundary = new Boundary(Camera.main, _settings.Margin);

            _signal.Subscribe<GameStartedSignal>(OnGameStart);
            SpawnAllBricks();
        }

        private void OnGameStart()
        {
            _hasStarted = true;
            SpawnRandom();
        }

        private void SpawnAllBricks()
        {
            Brick dummy = _brickFactory.Create();
            dummy.gameObject.SetActive(false);

            Vector2 brickSize = dummy.GetComponent<SpriteRenderer>().bounds.size;
            Vector2 startPos = new Vector2(0, 0); //Assumption: Only Top half is allowed for spawning
            Vector2 offset = brickSize * 0.5f;

            while ((startPos.y + offset.y) < _boundary.Top)
            {
                if (dummy != null) //first one  is already created
                {
                    dummy.transform.position = startPos;
                    dummy = null;
                }
                else
                {
                    SpawnNew(startPos);
                }

                if (startPos.x != 0)// mirror brick, spawning on both sides
                {
                    SpawnNew(new Vector2(-startPos.x, startPos.y));
                }

                startPos.x += brickSize.x + _settings.Padding;
                if ((startPos.x + offset.y) > _boundary.Right)
                {
                    startPos.x = 0; //start new row
                    startPos.y += brickSize.y + _settings.Padding;
                }
            }
        }

        private void SpawnNew(Vector2 pos)
        {
            Brick dummy = _brickFactory.Create();
            dummy.transform.position = pos;
            dummy.gameObject.SetActive(false);
            _brickPool.Add(dummy);
        }

        public void Tick()
        {
            if (_hasStarted)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _settings.SpawnFrequency)
                {
                    _elapsedTime = 0;
                    SpawnRandom();
                }
            }
        }

        private void SpawnRandom()
        {
            List<int> idleIndices = new List<int>();
            for (int i = 0; i < _brickPool.Count; i++)
            {
                if (!_brickPool[i].gameObject.activeInHierarchy)
                {
                    idleIndices.Add(i);
                }
            }

            if (idleIndices.Count > 0)
            {
                _brickPool[idleIndices[Random.Range(0, idleIndices.Count)]].gameObject.SetActive(true);
            }
        }

        public void LateDispose()
        {
            _signal.Unsubscribe<GameStartedSignal>(OnGameStart);
        }

        //====Settings
        [System.Serializable]
        public class Settings
        {
            [Range(0.1f, 2)] public float Margin; //world co-ords
            [Range(0.1f, 2)] public float Padding; // world co-ords
            [Range(2, 20)] public float SpawnFrequency; // in seconds
        }
    }
}