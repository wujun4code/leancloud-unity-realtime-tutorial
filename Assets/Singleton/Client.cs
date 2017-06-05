using LeanCloud;
using LeanCloud.Realtime;

public class ClientSingleton
{
    private static AVIMClient _current;
    public static AVIMClient Current
    {
        get
        {
            return _current;
        }
    }

    public static void Init(AVIMClient client)
    {
        _current = client;
    }
}