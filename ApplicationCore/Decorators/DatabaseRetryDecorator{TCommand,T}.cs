﻿using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Utils;
using System;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators
{
    public sealed class DatabaseRetryDecorator<TCommand,T> : ICommandHandler<TCommand,T>
        where TCommand : ICommand<T>
    {
        private readonly ICommandHandler<TCommand,T> _handler;
        private readonly Config _config;

        public DatabaseRetryDecorator(ICommandHandler<TCommand,T> handler, Config config)
        {
            _config = config;
            _handler = handler;
        }

        public async Task<Result<T>> Handle(TCommand command)
        {
            for (int i = 0; ; i++)
            {
                try
                {
                    Result<T> result = await _handler.Handle(command);
                    return result;
                }
                catch (Exception ex)
                {
                    if (i >= _config.NumberOfDatabaseRetries || !IsDatabaseException(ex))
                        throw;
                }
            }
        }

        private bool IsDatabaseException(Exception exception)
        {
            string message = exception.InnerException?.Message;

            if (message == null)
                return false;

            return message.Contains("The connection is broken and recovery is not possible")
                || message.Contains("error occurred while establishing a connection");
        }
    }
}
