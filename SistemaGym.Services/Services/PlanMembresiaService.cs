using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.Interfaces;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using System.Net;
using SistemaGym.Core.CustomEntities;
using SistemaGym.Core.Enum;

namespace SistemaGym.Services.Services
{
    public class PlanMembresiaService : IPlanMembresiaService
    {
        public readonly IUnitOfWork _unitOfWork;

        public PlanMembresiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PlanMembresia>> GetAllPlanesAsync(
            PlanMembresiaQueryFilter? filters = null)
        {
            var planes = await _unitOfWork.PlanMembresiaRepository.GetAll();

            if (filters != null)
            {
                if (!string.IsNullOrWhiteSpace(filters.NombrePlan))
                {
                    planes = planes.Where(p =>
                        p.NombrePlan != null &&
                        p.NombrePlan.ToLower().Contains(filters.NombrePlan.ToLower()));
                }

                if (filters.DuracionDias != null)
                {
                    planes = planes.Where(p =>
                        p.DuracionDias == filters.DuracionDias);
                }

                if (filters.PrecioMin != null)
                {
                    planes = planes.Where(p =>
                        p.Precio >= filters.PrecioMin);
                }

                if (filters.PrecioMax != null)
                {
                    planes = planes.Where(p =>
                        p.Precio <= filters.PrecioMax);
                }

                if (filters.Estado != null)
                {
                    planes = planes.Where(p =>
                        p.Estado == filters.Estado);
                }
            }

            return planes;
        }

        public async Task<ResponseData> GetAllPlanesResponseAsync(
            PlanMembresiaQueryFilter? filters = null)
        {
            filters ??= new PlanMembresiaQueryFilter();

            var planes = await GetAllPlanesAsync(filters);
            var pagedPlanes = PagedList<object>
                .Create(planes.Cast<object>(), filters.PageNumber, filters.PageSize);

            if (pagedPlanes.Any())
            {
                return new ResponseData
                {
                    Messages = new Message[]
                    {
                        new()
                        {
                            Type = TypeMessage.success.ToString(),
                            Description = "Registros de planes recuperados correctamente"
                        }
                    },
                    Pagination = pagedPlanes,
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
                        Description = "No fue posible recuperar registros de planes"
                    }
                },
                Pagination = pagedPlanes,
                StatusCode = HttpStatusCode.NotFound
            };
        }

        public async Task<IEnumerable<PlanMembresia>> GetAllPlanesDapperAsync(
            int limit = 10)
        {
            return await _unitOfWork.PlanMembresiaRepository
                .GetAllPlanesDapperAsync(limit);
        }

        public async Task<PlanMembresia> GetPlanByIdAsync(int id)
        {
            return await _unitOfWork.PlanMembresiaRepository.GetById(id);
        }

        public async Task InsertPlan(PlanMembresia plan)
        {
            if (string.IsNullOrWhiteSpace(plan.NombrePlan))
            {
                throw new BussinesException(
                    "El nombre del plan es obligatorio.",
                    HttpStatusCode.BadRequest);
            }

            if (!plan.Precio.HasValue || plan.Precio <= 0)
            {
                throw new BussinesException(
                    "El precio debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            if (!plan.DuracionDias.HasValue || plan.DuracionDias <= 0)
            {
                throw new BussinesException(
                    "La duración debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            var planes = await _unitOfWork.PlanMembresiaRepository.GetAll();

            if (planes.Any(p =>
                p.NombrePlan != null &&
                p.NombrePlan.ToLower() == plan.NombrePlan.ToLower()))
            {
                throw new BussinesException(
                    "Ya existe un plan de membresía con ese nombre.",
                    HttpStatusCode.BadRequest);
            }

            await _unitOfWork.PlanMembresiaRepository.Add(plan);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdatePlan(PlanMembresia plan)
        {
            if (string.IsNullOrWhiteSpace(plan.NombrePlan))
            {
                throw new BussinesException(
                    "El nombre del plan es obligatorio.",
                    HttpStatusCode.BadRequest);
            }

            if (!plan.Precio.HasValue || plan.Precio <= 0)
            {
                throw new BussinesException(
                    "El precio debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            if (!plan.DuracionDias.HasValue || plan.DuracionDias <= 0)
            {
                throw new BussinesException(
                    "La duración debe ser mayor a cero.",
                    HttpStatusCode.BadRequest);
            }

            var planes = _unitOfWork.PlanMembresiaRepository.GetAll().Result;

            if (planes.Any(p =>
                p.NombrePlan != null &&
                p.NombrePlan.ToLower() == plan.NombrePlan.ToLower() &&
                p.Id != plan.Id))
            {
                throw new BussinesException(
                    "Ya existe otro plan de membresía con ese nombre.",
                    HttpStatusCode.BadRequest);
            }

            _unitOfWork.PlanMembresiaRepository.Update(plan);
            _unitOfWork.SaveChanges();
        }

        public async Task DeletePlan(int id)
        {
            var plan = await _unitOfWork.PlanMembresiaRepository.GetById(id);

            if (plan == null)
            {
                throw new BussinesException(
                    "El plan de membresía no existe.",
                    HttpStatusCode.NotFound);
            }

            var membresias = await _unitOfWork.MembresiaRepository.GetAll();

            bool tieneMembresias = membresias.Any(m => m.PlanMembresiaId == id);

            if (tieneMembresias)
            {
                throw new BussinesException(
                    "No se puede eliminar el plan porque ya tiene membresías asignadas. Puede desactivarlo cambiando su estado a false.",
                    HttpStatusCode.BadRequest);
            }

            await _unitOfWork.PlanMembresiaRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
