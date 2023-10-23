namespace ZuckMT;

public class ResourceLock
{
    public static object Lock = new();
    private static bool _resourceReady;
    private static TaskCompletionSource<bool> _compleitionSource = null!;

    public static Task WaitForResourceAsync()
    {
        lock (Lock)
        {
            if (_resourceReady)
            {
                return Task.CompletedTask;
            }

            _compleitionSource = new TaskCompletionSource<bool>();
        }

        return _compleitionSource.Task;
    }

    public static void ResourceAvailable()
    {
        lock (Lock)
        {
            if (!_resourceReady)
            {
                _resourceReady = true;
                _compleitionSource?.SetResult(true);
            }
        }
    }
}