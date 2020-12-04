using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{

    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
                                        string filePath)
        {
            factory.AddProvider(new FileLoggerProvider(filePath));
            return factory;
        }
        public static ILoggerFactory AddErrorFile(this ILoggerFactory factory,
                                                    string filePath)
        {
            factory.AddProvider(new ErrorFileLoggerProvider(filePath));
            return factory;
        }
    }
}
