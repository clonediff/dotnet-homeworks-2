using System;
using System.Threading;

namespace Hw3.Tests;

public class SingleInitializationSingleton
{
    private static readonly object Locker = new();

    private static volatile bool _isInitialized = false;

    public const int DefaultDelay = 3_000;

    public static Lazy<SingleInitializationSingleton> instance = new(() => new());
    public int Delay { get; }

    private SingleInitializationSingleton(int delay = DefaultDelay)
    {
        Delay = delay;
        // imitation of complex initialization logic
        Thread.Sleep(delay);
    }

    internal static void Reset()
    {
        lock (Locker)
        {
            _isInitialized = false;
            instance = new(() => new());
        }
    }

    public static void Initialize(int delay)
    {
        if (!_isInitialized)
        {
            lock (Locker)
            {
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    instance = new(() => new(delay));
                    return;
                }
            }
        }
        throw new InvalidOperationException();
    }

    public static SingleInitializationSingleton Instance => instance.Value;

}