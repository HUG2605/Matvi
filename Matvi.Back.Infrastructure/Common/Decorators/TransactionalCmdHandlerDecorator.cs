using Matvi.Back.Infrastructure.Common.Attributes;
using Matvi.Back.Infrastructure.Persistence.Repositories;
using MediatR;
using System.Reflection;

namespace Matvi.Back.Infrastructure.Common.Decorators
{
    /// <summary>
    /// Decorador para manejadores de comandos de MediatR que envuelve la ejecución en una transacción de base de datos si la clase está decorada con <see cref="TransactionAttribute"/>.
    /// </summary>
    /// <typeparam name="TRequest">Tipo de la solicitud que implementa <see cref="IRequest{TResponse}"/>.</typeparam>
    /// <typeparam name="TResponse">Tipo de la respuesta producida por el manejador.</typeparam>
    public class TransactionalCmdHandlerDecorator<TRequest, TResponse>(
        IRequestHandler<TRequest, TResponse> _inner,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        /// <inheritdoc/>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var handlerType = _inner.GetType();
            var hasAttribute = handlerType.GetCustomAttribute<TransactionAttribute>() != null;

            if (!hasAttribute)
                return await _inner.Handle(request, cancellationToken);

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var response = await _inner.Handle(request, cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitAsync(cancellationToken);

                return response;
            }
            catch
            {
                await _unitOfWork.RollbackAsync(cancellationToken);
                throw; // UnitOfWork ya parsea las excepciones
            }
        }
    }
}
