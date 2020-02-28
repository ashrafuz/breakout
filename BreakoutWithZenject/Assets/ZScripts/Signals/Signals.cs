using UnityEngine;

namespace WithZenject {
    public class GameStartedSignal { }
    public class BallCollidedSignal {
        public GameObject CollidedWith;
    }

    public class NewScoreUpdateSignal {
        public int NewScore;
    }
}