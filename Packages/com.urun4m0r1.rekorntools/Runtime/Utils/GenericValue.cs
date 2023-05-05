#nullable enable

namespace Urun4m0r1.RekornTools.Utils
{
    /// <summary>
    /// Use this struct to solve generic classes static field conflict.
    /// <a href="https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1000">CA1000</a>
    /// </summary>
    // ReSharper disable once UnusedTypeParameter
    public struct GenericValue<TClass, TValue>
    {
        public TValue? Value { get; set; }

        public GenericValue(TValue? value)
        {
            Value = value;
        }
    }
}
