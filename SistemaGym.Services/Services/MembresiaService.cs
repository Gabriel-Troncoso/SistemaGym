using SistemaGym.Core.Entities;
using SistemaGym.Core.Exceptions;
using SistemaGym.Core.Interfaces;
using SistemaGym.Core.QueryFilters;
using SistemaGym.Services.Interfaces;
using System.Net;

namespace SistemaGym.Services.Services
{
    public class MembresiaService : IMembresiaService
    {
        public readonly IUnitOfWork _unitOfWork;

        public MembresiaService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Membresia>> GetAllMembresiasAsync(
            MembresiaQueryFilter? filters = null)
        {
            var membresias = await _unitOfWork.MembresiaRepository.GetAll();

            if (filters != null)
            {
                if (filters.ClienteId != null)
                {
                    membresias = membresias.Where(m =>
                        m.ClienteId == filters.ClienteId);
                }

                if (filters.PlanMembresiaId != null)
                {
                    membresias = membresias.Where(m =>
                        m.PlanMembresiaId == filters.PlanMembresiaId);
                }

                if (filters.FechaInicio != null)
                {
                    membresias = membresias.Where(m =>
                        m.FechaInicio >= filters.FechaInicio);
                }

                if (filters.FechaFin != null)
                {
                    membresias = membresias.Where(m =>
                        m.FechaFin <= filters.FechaFin);
                }

                if (filters.Estado != null)
                {
                    membresias = membresias.Where(m =>
                        m.Estado == filters.Estado);
                }
            }

            return membresias;
        }

        public async Task<IEnumerable<Membresia>> GetAllMembresiasDapperAsync(
            int limit = 10)
        {
            return await _unitOfWork.MembresiaRepository
                .GetAllMembresiasDapperAsync(limit);
        }

        public async Task<Membresia> GetMembresiaByIdAsync(int id)
        {
            return await _unitOfWork.MembresiaRepository.GetById(id);
        }

        public async Task InsertMembresia(Membresia membresia)
        {
            var cliente = await _unitOfWork.ClienteRepository
                .GetById(membresia.ClienteId);

            if (cliente == null)
            {
                throw new BussinesException(
                    "El cliente no existe.",
                    HttpStatusCode.BadRequest);
            }

            var plan = await _unitOfWork.PlanMembresiaRepository
                .GetById(membresia.PlanMembresiaId);

            if (plan == null)
            {
                throw new BussinesException(
                    "El plan de membresía no existe.",
                    HttpStatusCode.BadRequest);
            }

            if (membresia.FechaInicio.HasValue &&
                membresia.FechaFin.HasValue &&
                membresia.FechaFin < membresia.FechaInicio)
            {
                throw new BussinesException(
                    "La fecha fin no puede ser menor que la fecha inicio.",
                    HttpStatusCode.BadRequest);
            }

            var membresias = await _unitOfWork.MembresiaRepository.GetAll();

            bool clienteTieneMembresiaActiva = membresias.Any(m =>
                m.ClienteId == membresia.ClienteId &&
                m.Estado == true &&
                (!m.FechaFin.HasValue || m.FechaFin >= DateTime.Now));

            if (clienteTieneMembresiaActiva)
            {
                throw new BussinesException(
                    "El cliente ya tiene una membresía activa.",
                    HttpStatusCode.BadRequest);
            }

            await _unitOfWork.MembresiaRepository.Add(membresia);
            await _unitOfWork.SaveChangesAsync();
        }

        public void UpdateMembresia(Membresia membresia)
        {
            var cliente = _unitOfWork.ClienteRepository
                .GetById(membresia.ClienteId).Result;

            if (cliente == null)
            {
                throw new BussinesException(
                    "El cliente no existe.",
                    HttpStatusCode.BadRequest);
            }

            var plan = _unitOfWork.PlanMembresiaRepository
                .GetById(membresia.PlanMembresiaId).Result;

            if (plan == null)
            {
                throw new BussinesException(
                    "El plan de membresía no existe.",
                    HttpStatusCode.BadRequest);
            }

            if (membresia.FechaInicio.HasValue &&
                membresia.FechaFin.HasValue &&
                membresia.FechaFin < membresia.FechaInicio)
            {
                throw new BussinesException(
                    "La fecha fin no puede ser menor que la fecha inicio.",
                    HttpStatusCode.BadRequest);
            }

            var membresias = _unitOfWork.MembresiaRepository.GetAll().Result;

            bool clienteTieneOtraMembresiaActiva = membresias.Any(m =>
                m.Id != membresia.Id &&
                m.ClienteId == membresia.ClienteId &&
                m.Estado == true &&
                (!m.FechaFin.HasValue || m.FechaFin >= DateTime.Now));

            if (clienteTieneOtraMembresiaActiva)
            {
                throw new BussinesException(
                    "El cliente ya tiene otra membresía activa.",
                    HttpStatusCode.BadRequest);
            }

            _unitOfWork.MembresiaRepository.Update(membresia);
            _unitOfWork.SaveChanges();
        }

        public async Task DeleteMembresia(int id)
        {
            var membresia = await _unitOfWork.MembresiaRepository.GetById(id);

            if (membresia == null)
            {
                throw new BussinesException(
                    "La membresía no existe.",
                    HttpStatusCode.NotFound);
            }

            await _unitOfWork.MembresiaRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}