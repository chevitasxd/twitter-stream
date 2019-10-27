using System;
using System.Threading;
using System.Threading.Tasks;

namespace backend_stream
{
    public interface ITweetHandler
    {        
        event EventHandler<string> OnTweetEventHandled;
        void Connect();
        void Disconnect();
    }
}