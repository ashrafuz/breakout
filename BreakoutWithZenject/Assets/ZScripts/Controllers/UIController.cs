using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Zenject;

namespace WithZenject {

    public class UIController : MonoBehaviour, ILateDisposable {
        [SerializeField] private TextMeshProUGUI m_ScoreText;
        [SerializeField] private GameObject m_WelcomePanel;

        private SignalBus _signalBus;
        private bool _hasStarted;
        private int _uiScore = 0;
        private int _finalScore = 0;
        private float _elapsedTime;

        private Settings _settings;

        [Inject]
        public void Construct (SignalBus signal, Settings settings) {
            _signalBus = signal;
            _settings = settings;
        }

        public void LateDispose () {
            _signalBus.Unsubscribe<GameStartedSignal> (OnGameStart);
            _signalBus.Unsubscribe<NewScoreUpdateSignal> (OnNewScoreUpdate);
        }

        private void Start () {
            m_ScoreText.text = string.Empty;
            m_WelcomePanel.SetActive (!_hasStarted);

            _signalBus.Subscribe<GameStartedSignal> (OnGameStart);
            _signalBus.Subscribe<NewScoreUpdateSignal> (OnNewScoreUpdate);
        }

        private void OnNewScoreUpdate (NewScoreUpdateSignal nSignal) {
            _finalScore = nSignal.NewScore;
        }

        private void OnGameStart () {
            _hasStarted = true;
            m_WelcomePanel.SetActive (!_hasStarted);
        }
        private void UpdateScoreText () {
            m_ScoreText.text = "Score : " + _uiScore;
        }

        private void Update () {
            if (_uiScore <= _finalScore) { //we have to animate
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _settings.ScoreUpdateFrequency) {
                    _elapsedTime = 0;
                    _uiScore = Mathf.Clamp (_uiScore + _settings.ScoreUpdateRate, 0, _finalScore);
                    UpdateScoreText ();
                }
            }
        }

        [System.Serializable]
        public class Settings {
            public int ScoreUpdateRate;
            [Range (0.001f, 0.25f)] public float ScoreUpdateFrequency;
        }

    }

}