using Matvi.Back.Application.Modules.Productos.Producto.Commands;
using Matvi.Back.Application.Modules.Productos.Producto.Dtos;
using Matvi.Back.Application.Modules.Productos.Producto.Queries;
using Matvi.Back.Application.Modules.Productos.Seccion.Queries;
using Matvi.Back.Application.Modules.Productos.UnidadBaseInventario.Queries;
using Matvi.Front.UI.Attributes;
using Matvi.Front.UI.Security.Permission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Matvi.Front.UI.Areas.Productos.Pages.Producto
{
        /// <summary>
    /// Modelo de página Razor (PageModel) que gestiona la interacción para create.
    /// </summary>
[AuthorizeMatvi(Funcion.Productos_Productos, Accion.Crear)]
    public class CreateModel(
        IMediator _mediator
        ) : BaseModel
    {
                /// <summary>
        /// Propiedad que representa producto.
        /// </summary>
[BindProperty]
        public required CrearProductoDto Producto { get; set; }

                /// <summary>
        /// Propiedad que representa seccion select list.
        /// </summary>
public SelectList SeccionSelectList { get; set; } = default!;

                /// <summary>
        /// Propiedad que representa grupo select list.
        /// </summary>
public SelectList GrupoSelectList { get; set; } = default!;

                /// <summary>
        /// Propiedad que representa unidad base inventario select list.
        /// </summary>
public SelectList UnidadBaseInventarioSelectList { get; set; } = default!;

        //Imagen varchar	100	yes
                /// <summary>
        /// Propiedad que representa imagen.
        /// </summary>
[Display(Name = "Imagen")]
        [BindProperty]
        public IFormFile? ImagenFld { get; set; }

                /// <inheritdoc/>
public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var productoTask = _mediator.Send(
                new CrearProductoQry(), 
                cancellationToken);
            var seccionesTask = _mediator.Send(
                new ListarSeccionSelectQry(), 
                cancellationToken);
            var unidadesBaseInventarioTask = _mediator.Send(
                new ListarUnidadBaseInventarioSelectQry(), 
                cancellationToken);

            await Task.WhenAll(productoTask, seccionesTask, unidadesBaseInventarioTask);

            Producto = productoTask.Result;
            SeccionSelectList = new SelectList(seccionesTask.Result, "Id", "Nombre");
            UnidadBaseInventarioSelectList = new SelectList(unidadesBaseInventarioTask.Result, "Id", "Nombre");
        }

                /// <summary>
        /// Ejecuta la operación de on post create async.
        /// </summary>
        /// <param name="cancellationToken">Token de cancelación para abortar la operación asíncrona.</param>
        /// <returns>Tarea asíncrona con el resultado de la operación.</returns>
public async Task<JsonResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            if (ImagenFld != null && ImagenFld.Length > 0)
            {
                using var memoryStream = new MemoryStream();

                await ImagenFld.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();
                Producto.ImagenFld = Convert.ToBase64String(imageBytes);
            }

            var res = await _mediator.Send(
                new CrearProductoCmd(Producto), 
                cancellationToken);

            return new JsonResult(res);
        }
    }
}
