using UnityEngine;
using Zenject;

namespace WithZenject {

    public class GameInstaller : MonoInstaller {

        [Inject]
        private Settings _settings;

        public override void InstallBindings () {

            Container.BindInterfacesAndSelfTo<GameController> ().AsSingle ().NonLazy ();
            InstallBallSystem ();
            InstallBrickSystem ();
            InstallSignals ();
        }

        void InstallBrickSystem () {
            //InstallBrickManager
            Container.BindInterfacesAndSelfTo<BrickManager> ().AsSingle ().NonLazy ();
            Container.BindFactory<Brick, Brick.Factory> ()
                .FromComponentInNewPrefab (_settings.BrickPrefab)
                .WithGameObjectName ("_brick")
                .UnderTransformGroup ("Bricks");
        }

        void InstallBallSystem () {
            //InstallBalls
            Container.BindInterfacesAndSelfTo<BallManager> ().AsSingle ().NonLazy ();
            Container.BindFactory<Ball, Ball.Factory> ()
                .FromComponentInNewPrefab (_settings.BallPrefab)
                .WithGameObjectName ("_ball")
                .UnderTransformGroup ("Balls");
        }

        void InstallSignals () {
            SignalBusInstaller.Install (Container);
            Container.DeclareSignal<GameStartedSignal> ();
            Container.DeclareSignal<BallCollidedSignal> ();
            Container.DeclareSignal<NewScoreUpdateSignal> ();
        }

        [System.Serializable]
        public class Settings {
            public GameObject BallPrefab;
            public GameObject BrickPrefab;
        }
    }
}