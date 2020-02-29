using UnityEngine;

public struct Boundary
{
    public float Left;
    public float Right;
    public float Top;
    public float Bot;

    public Boundary(Camera mc, float padding = 0)
    {
        Left = Helper.GetLeftBoundary(mc) + padding;
        Right = Helper.GetRightBoundary(mc) - padding;
        Top = Helper.GetTopBoundary(mc) - padding;
        Bot = Helper.GetBotBoundary(mc) + padding;
    }
}

public static class Helper
{

    public static float GetLeftBoundary(Camera mainCam)
    {
        return mainCam.ScreenToWorldPoint(Vector2.zero).x;
    }

    public static float GetRightBoundary(Camera mainCam)
    {
        return mainCam.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    public static float GetTopBoundary(Camera mainCam)
    {
        return mainCam.ScreenToWorldPoint(new Vector2(0, Screen.height)).y;
    }

    public static float GetBotBoundary(Camera mainCam)
    {
        return mainCam.ScreenToWorldPoint(Vector2.zero).y;
    }

}