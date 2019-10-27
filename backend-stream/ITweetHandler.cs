using System;

namespace backend_stream
{
    public interface ITweetHandler
    {        
        event EventHandler<string> OnTweetEventHandled;
        void Connect();
        void Disconnect();
    }
}