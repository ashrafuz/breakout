using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WithoutDI
{
    public class BrickSpawner : MonoBehaviour
    {
        [SerializeField] [Range(0.1f, 2)] private float m_Margin;
        [SerializeField] [Range(0.1f, 2)] private float m_Padding;

        [SerializeField] [Range(2, 20)] private float m_SpawnFrequecyInSeconds;
        [SerializeField] private Brick m_BrickPrefab;

        private Boundary _boundary;
        private float _elapsedTime = 0;

        private List<Brick> _brickPool = new List<Brick>();

        private GameManager _gameManager;

        void Start()
        {
            InitalizeBoundary();
            SpawnAllBricks();
            GameManager.OnGameStart += OnGameStart;
        }

        private void SpawnAllBricks()
        {
            Vector2 brickSize = m_BrickPrefab.GetComponent<SpriteRenderer>().bounds.size;
            //Assumption: Only Top half is allowed for spawning
            Vector2 startPos = new Vector2(0, 0);
            Vector2 offset = brickSize * 0.5f;

            while ((startPos.y + offset.y) < _boundary.Top)
            {
                _brickPool.Add(SpawnNewAt(startPos));
                if (startPos.x != 0)
                {
                    _brickPool.Add(SpawnNewAt(new Vector2(-startPos.x, startPos.y)));
                }

                startPos.x += brickSize.x + m_Padding;
                if ((startPos.x + offset.y) > _boundary.Right)
                {
                    startPos.x = 0; //start new row
                    startPos.y += brickSize.y + m_Padding;
                }
            }
        }

        private void InitalizeBoundary()
        {
            //Assumption: Horizontal & Vertical margin is same
            Camera mc = Camera.main;
            _boundary = new Boundary(mc, m_Margin);
        }

        private void OnGameStart(GameManager gm)
        {
            _gameManager = gm;
            SpawnRandom();
        }

        private void Update()
        {
            if (_gameManager == null) { return; }

            if (_gameManager.IsRunning)
            {
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= m_SpawnFrequecyInSeconds)
                {
                    _elapsedTime = 0;
                    SpawnRandom();
                }
            }
        }

        private Brick SpawnNewAt(Vector2 pos, bool hide = true)
        {
            Brick brickToSpawn = Instantiate(m_BrickPrefab, this.transform);
            brickToSpawn.transform.position = (Vector2)pos;
            brickToSpawn.gameObject.SetActive(!hide);

            _brickPool.Add(brickToSpawn);
            return brickToSpawn;
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
    }
}