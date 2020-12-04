using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    class ErrorFileLoggerProvider : ILoggerProvider
    {
        string _path;
        public ErrorFileLoggerProvider(string path)
        {
            _path = path;
        }
        public ILogger CreateLogger(string categoryName)
        {
            return new  ErrorFileLogger(_path);
        }

        public void Dispose()
        {
     
        }
    }
}
