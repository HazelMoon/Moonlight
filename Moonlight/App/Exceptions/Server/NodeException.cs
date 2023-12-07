namespace Moonlight.App.Exceptions.Server;

public class NodeException : Exception
{
    public NodeException()
    {
    }

    public NodeException(string message) : base(message)
    {
    }

    public NodeException(string message, Exception inner) : base(message, inner)
    {
    }
}