using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace WithoutDI {

    public class UIManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private GameObject m_WelcomePanel;
        [SerializeField] private int m_ScoreUpdateRate;
        [SerializeField][Range (0.001f, 0.25f)] private float m_ScoreUpdateFrequency;

        private int _uiScore = 0;
        private float _elapsedTime;

        private GameManager _gameManager;

        void Start () {
            GameManager.OnGameStart += OnGameStart;

            m_Score.text = string.Empty;
            m_WelcomePanel.gameObject.SetActive (true);
        }

        private void OnGameStart (GameManager gm) {
            _gameManager = gm;
            m_WelcomePanel.gameObject.SetActive (false);
        }

        private void UpdateScoreText () {
            m_Score.text = "Score : " + _uiScore;
        }

        private void Update () {
            if (_gameManager == null) {
                return;
            }

            if (_uiScore <= _gameManager.Score) { //we have to animate
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= m_ScoreUpdateFrequency) {
                    _elapsedTime = 0;
                    _uiScore = Mathf.Clamp (_uiScore + m_ScoreUpdateRate, 0, _gameManager.Score);
                    UpdateScoreText ();
                }
            }
        }

    }
}