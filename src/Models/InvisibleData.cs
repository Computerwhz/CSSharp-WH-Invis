namespace CSSharpWHInvi.Models;

public struct InvisibleData(float startTime = -1f, float endTime = -1f)
{
    public float StartTime = startTime;
    public float EndTime = endTime;
    public bool HackyReload = false;
}