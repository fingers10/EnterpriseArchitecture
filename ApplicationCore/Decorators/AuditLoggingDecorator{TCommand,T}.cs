using CSharpFunctionalExtensions;
using Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Decorators
{
    public sealed class AuditLoggingDecorator<TCommand, T> : ICommandHandler<TCommand, T>
        where TCommand : ICommand<T>
    {
        private readonly ICommandHandler<TCommand, T> _handler;

        public AuditLoggingDecorator(ICommandHandler<TCommand, T> handler)
        {
            _handler = handler;
        }

        public async Task<Result<T>> Handle(TCommand command)
        {
            string commandJson = JsonSerializer.Serialize(command);

            // Use proper logging here
            Console.WriteLine($"Command of type {command.GetType().Name}: {commandJson}");

            return await _handler.Handle(command);
        }
    }
}
