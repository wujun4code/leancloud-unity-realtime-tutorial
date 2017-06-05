using LeanCloud;
using LeanCloud.Realtime;

public class RealtimeSingleton
{
    private static AVRealtime _current;
    public static AVRealtime Current
    {
        get
        {
            return _current;
        }
    }

    public static void Init(AVRealtime realtime)
    {
        _current = realtime;
    }
}