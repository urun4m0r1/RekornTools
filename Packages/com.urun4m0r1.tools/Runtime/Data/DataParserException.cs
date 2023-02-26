using System;

namespace Urun4m0r1.RekornTools.Data
{
    public sealed class DataParserException : Exception
    {
        public DataParserException() { }
        public DataParserException(string message) : base(message) { }
        public DataParserException(string message, Exception inner) : base(message, inner) { }
    }
}
