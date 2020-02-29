using UnityEngine;
using Zenject;

namespace WithZenject
{
    [CreateAssetMenu(fileName = "GameSettingInstaller", menuName = "Installers/GameSettingInstaller")]
    public class GameSettingInstaller : ScriptableObjectInstaller<GameSettingInstaller>
    {

        public GameInstaller.Settings GameInstaller;
        public BallSettings BallSetting;
        public BrickSettings BrickSetting;
        public GameController.Setting GameController;
        public UIController.Settings UiController;

        public override void InstallBindings()
        {
            Container.BindInstance(BallSetting.Spawner);
            Container.BindInstance(BallSetting.Individual);
            Container.BindInstance(BrickSetting.Spawner);
            Container.BindInstance(BrickSetting.Individual);
            Container.BindInstance(GameInstaller);
            Container.BindInstance(GameController);
            Container.BindInstance(UiController);
        }

        [System.Serializable]
        public class BrickSettings
        {
            public BrickManager.Settings Spawner;
            public Brick.Settings Individual;
        }

        [System.Serializable]
        public class BallSettings
        {
            public BallManager.Settings Spawner;
            public Ball.Settings Individual;
        }
    }
}