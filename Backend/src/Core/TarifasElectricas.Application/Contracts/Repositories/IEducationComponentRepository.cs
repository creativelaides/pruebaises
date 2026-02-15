using System;
using TarifasElectricas.Application.Contracts.Repositories.Generic;
using TarifasElectricas.Domain.Entities;

namespace TarifasElectricas.Application.Contracts.Repositories;

/// <summary>
/// Interfaz específica para operaciones de persistencia de EducationComponent
/// </summary>
public interface IEducationComponentRepository : IRepository<EducationComponent>
{
    Task<EducationComponent?> GetByCodeAsync(string code);
    Task<IEnumerable<EducationComponent>> GetByOrderAsync(int order);
    Task<bool> CodeExistsAsync(string code);
}