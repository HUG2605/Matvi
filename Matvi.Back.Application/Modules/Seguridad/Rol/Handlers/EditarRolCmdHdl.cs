    using Matvi.Back.Application.Common.Results;
    using Matvi.Back.Application.Modules.Seguridad.Rol.Commands;
    using Matvi.Back.Infrastructure.Common.Attributes;
    using Matvi.Back.Infrastructure.Persistence.Entities;
    using Matvi.Back.Infrastructure.Persistence.Repositories;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    namespace Matvi.Back.Application.Modules.Seguridad.Rol.Handlers
    {
        [Transaction]
        class EditarRolCmdHdl(
            IUnitOfWork _unitOfWork
            ) : IRequestHandler<EditarRolCmd, OperationResult<bool>>
        {
                        /// <inheritdoc/>
public async Task<OperationResult<bool>> Handle(
                EditarRolCmd request, 
                CancellationToken cancellationToken)
            {
                var rolId = request.rol.Id;

                // 1. Cargar TODAS las funciones del rol
                var funcionesEtt = await _unitOfWork.Query<RlRolFuncionEtt>()
                    .Where(f => f.IdCtRol == rolId)
                    .ToListAsync(cancellationToken);

                // 2. Cargar TODAS las acciones del rol
                var accionesEtt = await _unitOfWork.Query<RlRolAccionEtt>()
                    .Where(a => a.IdCtRol == rolId)
                    .ToListAsync(cancellationToken);

                // 3. Indexar DTOs de funciones por Id (Id de RlRolFuncion)
                var funcionesDto = request.rol.Funciones!.ToDictionary(f => f.Id);

                // 4. Indexar acciones de BD por Id (Id de RlRolAccion)
                var accionesEttPorId = accionesEtt.ToDictionary(a => a.Id);

                // 5. Actualizar funciones y sus acciones
                foreach (var funEtt in funcionesEtt)
                {
                    var funDto = funcionesDto[funEtt.Id];

                    // Actualizar flags de función
                    funEtt.Consultar = funDto.Consultar;
                    funEtt.VerSensibles = funDto.VerSensibles;
                    _unitOfWork.Update(funEtt);

                    if (funDto.Acciones is null || funDto.Acciones.Count == 0)
                        continue;

                    // Actualizar acciones de esta función usando el Id directo
                    foreach (var accDto in funDto.Acciones)
                    {
                        if (!accionesEttPorId.TryGetValue(accDto.Id, out var accEtt))
                            continue; // Si por alguna razón no existe en BD, lo saltamos

                        accEtt.TieneAcceso = accDto.TieneAcceso;
                        _unitOfWork.Update(accEtt);
                    }
                }

                return OperationResult<bool>.Success(
                    true, 
                    "El rol se editó correctamente.");
            }
        }
    }
