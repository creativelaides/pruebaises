using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TarifasElectricas.Domain.Entities;
using TarifasElectricas.Application.Contracts.Repositories.Generic;

namespace TarifasElectricas.Application.Contracts.Repositories;

/// <summary>
/// Repositorio específico para Company
/// 
/// Responsabilidad:
/// - Obtener empresas por código (operador_de_red de Gov.co)
/// - Verificar existencia de empresas
/// - Obtener todas las empresas activas
/// 
/// Métodos heredados de IRepository<T>:
/// - GetByIdAsync(id)
/// - GetAllAsync()
/// - AddAsync(entity)
/// - UpdateAsync(entity)
/// - DeleteAsync(entity)
/// - ExistsAsync(id)
/// </summary>
public interface ICompanyRepository : IRepository<Company>
{
    /// <summary>
    /// Obtiene una empresa por su código (operador_de_red)
    /// 
    /// Parámetro:
    ///   code: Código del operador (ej: "ENEL Bogotá - Cundinamarca")
    /// 
    /// Retorna:
    ///   Company si existe, null si no
    /// </summary>
    Task<Company?> GetByCodeAsync(string code);

    /// <summary>
    /// Verifica si un código de empresa existe en BD
    /// 
    /// Parámetro:
    ///   code: Código del operador a verificar
    /// 
    /// Retorna:
    ///   true si existe, false si no
    /// </summary>
    Task<bool> CodeExistsAsync(string code);

    /// <summary>
    /// Obtiene todas las empresas de la BD
    /// </summary>
    Task<IEnumerable<Company>> GetAllCompaniesAsync();
}