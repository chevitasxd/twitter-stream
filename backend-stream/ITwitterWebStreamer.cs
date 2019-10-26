using System;

namespace backend_stream
{
    public interface ITwitterWebStreamer
    {
        event EventHandler<string> OnTweetHandled;
    }
}