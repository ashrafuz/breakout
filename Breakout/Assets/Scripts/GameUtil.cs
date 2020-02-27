using UnityEngine;

public struct Boundary {

    public float Left;
    public float Right;
    public float Top;
    public float Bot;

    public Boundary (Camera mc, float padding = 0) {
        Left = GameUtil.GetLeftBoundary (mc) + padding;
        Right = GameUtil.GetRightBoundary (mc) - padding;
        Top = GameUtil.GetTopBoundary (mc) - padding;
        Bot = GameUtil.GetBotBoundary (mc) + padding;
    }
}

public static class GameUtil {

    public static float GetLeftBoundary (Camera mainCam) {
        return mainCam.ScreenToWorldPoint (Vector2.zero).x;
    }

    public static float GetRightBoundary (Camera mainCam) {
        return mainCam.ScreenToWorldPoint (new Vector2 (Screen.width, 0)).x;
    }

    public static float GetTopBoundary (Camera mainCam) {
        return mainCam.ScreenToWorldPoint (new Vector2 (0, Screen.height)).y;
    }

    public static float GetBotBoundary (Camera mainCam) {
        return mainCam.ScreenToWorldPoint (Vector2.zero).y;
    }

}