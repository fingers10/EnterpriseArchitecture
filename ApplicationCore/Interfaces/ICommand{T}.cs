using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Fingers10.EnterpriseArchitecture.ApplicationCore.Interfaces
{
    public interface ICommand<T>
    {
    }

    public interface ICommandHandler<TCommand,T>
        where TCommand : ICommand<T>
    {
        Task<Result<T>> Handle(TCommand command);
    }
}
