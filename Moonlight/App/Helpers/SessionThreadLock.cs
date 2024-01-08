namespace Moonlight.App.Helpers;

// Explanation:
// This class is a one-per-user class which provides objects to put a 'lock' on.
// Using this class will prevent any accidental use of a non thread safe object when running
// stuff in multiple tasks like the lazy loader can do.
public class SessionThreadLock
{
    public object LazyLoader { get; } = new();
}