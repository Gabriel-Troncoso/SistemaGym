using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.Entities;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.Interfaces;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using System.Net;

namespace SistemaGym.Services.Services
{
    public class PagoService : IPagoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PagoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Pago>> GetAllPagosAsync(
            PagoQueryFilter? filters = null)
        {
            var pagos = await _unitOfWork.PagoRepository.GetAll();

            if (filters != null)
            {
                if (filters.MembresiaId != null)
                {
                    pagos = pagos.Where(p => p.MembresiaId == filters.MembresiaId);
                }

                if (filters.MontoMin != null)
                {
                    pagos = pagos.Where(p => p.Monto >= filters.MontoMin);
                }

                if (filters.MontoMax != null)
                {
                    pagos = pagos.Where(p => p.Monto <= filters.MontoMax);
                }

                if (filters.FechaPagoDesde != null)
                {
                    pagos = pagos.Where(p => p.FechaPago >= filters.FechaPagoDesde);
                }

                if (filters.FechaPagoHasta != null)
                {
                    pagos = pagos.Where(p => p.FechaPago <= filters.FechaPagoHasta);
                }

                if (!string.IsNullOrWhiteSpace(filters.MetodoPago))
                {
                    pagos = pagos.Where(p =>
                        p.MetodoPago != null &&
                        p.MetodoPago.ToLower().Contains(filters.MetodoPago.ToLower()));
                }

                if (filters.Estado != null)
                {
                    pagos = pagos.Where(p => p.Estado == filters.Estado);
                }
            }

            return pagos;
        }

        public async Task<ResponseData> GetAllPagosResponseAsync(
            PagoQueryFilter? filters = null)
        {
            filters ??= new PagoQueryFilter();

            var pagos = await GetAllPagosAsync(filters);
            var pagedPagos = PagedList<object>
                .Create(pagos.Cast<object>(), filters.PageNumber, filters.PageSize);

            if (pagedPagos.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[]
                    {
                        new()
                        {
                            Type = TypeMessage.success.ToString(),
                            Description = "Registros de pagos recuperados correctamente"
                        }
                    },
                    Pagination = pagedPagos,
                    StatusCode = HttpStatusCode.OK
                };
            }

            return new ResponseData
            {
                Messages = new Message[]
                {
                    new()
                    {
                        Type = TypeMessage.warning.ToString(),
                        Description = "No fue posible recuperar registros de pagos"
                    }
                },
                Pagination = pagedPagos,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public async Task<Pago> GetPagoByIdAsync(int id)
        {
            return await _unitOfWork.PagoRepository.GetById(id);
        }

        public async Task InsertPago(Pago pago)
        {
            var membresia = await _unitOfWork.MembresiaRepository.GetById(pago.MembresiaId);

            if (membresia == null)
            {
                throw new BussinesException(
                    "La membresia no existe.",
                    HttpStatusCode.BadRequest);
            }

            if (!pago.Monto.HasValue || pago.Monto <= 0)
            {
                throw new BussinesException(
                    "El monto del pago debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
            {
                throw new BussinesException(
                    "El metodo de pago es obligatorio.",
                    HttpStatusCode.BadRequest);
            }
 

            pago.FechaPago ??= DateTime.Now;
            pago.Estado ??= true;

            await _unitOfWork.PagoRepository.Add(pago);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePago(Pago pago)
        {
            var membresia = await _unitOfWork.MembresiaRepository.GetById(pago.MembresiaId);

            if (membresia == null)
            {
                throw new BussinesException(
                    "La membresia no existe.",
                    HttpStatusCode.BadRequest);
            }

            if (!pago.Monto.HasValue || pago.Monto <= 0)
            {
                throw new BussinesException(
                    "El monto del pago debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(pago.MetodoPago))
            {
                throw new BussinesException(
                    "El metodo de pago es obligatorio.",
                    HttpStatusCode.BadRequest);
            }

            _unitOfWork.PagoRepository.Update(pago);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePago(int id)
        {
            var pago = await _unitOfWork.PagoRepository.GetById(id);

            if (pago == null)
            {
                throw new BussinesException(
                    "El pago no existe.",
                    HttpStatusCode.NotFound);
            }

            await _unitOfWork.PagoRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
