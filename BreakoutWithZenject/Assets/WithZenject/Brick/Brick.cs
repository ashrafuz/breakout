using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ModestTree;
using Zenject;

namespace WithZenject
{
    public class Brick : MonoBehaviour
    {
        private Settings _settings;

        private SpriteRenderer _spriteRenderer;
        [Inject]
        public void Construct(Settings settings)
        {
            _settings = settings;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            gameObject.SetActive(false);
        }

        //class helpers
        [System.Serializable]
        public class Settings
        {
            public Color BrickColor;
        }

        public class Factory : PlaceholderFactory<Brick>
        {

        }
    }
}