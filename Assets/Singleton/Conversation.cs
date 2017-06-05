using LeanCloud;
using LeanCloud.Realtime;

public class ConversationSingleton
{
    private static AVIMConversation _current;
    public static AVIMConversation Current
    {
        get
        {
            return _current;
        }
        set
        {
            _current = value;
        }
    }

}