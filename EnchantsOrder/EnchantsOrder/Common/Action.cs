#if NET20 || SILVERLIGHT3_0 || WINDOWSPHONE7_0
namespace System
{
    internal delegate TResult Func<in T, out TResult>(T arg);
    internal delegate TResult Func<in T1, in T2, out TResult>(T1 arg1, T2 arg2);
}
#endif