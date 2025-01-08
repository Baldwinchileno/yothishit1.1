﻿using System;
using System.Data;
using System.Threading.Tasks;
using AdminSERMAC.Core.Interfaces;
using AdminSERMAC.Models;

namespace AdminSERMAC.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Manejo de transacciones
        IDbTransaction BeginTransaction();
        Task BeginTransactionAsync();
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();

        // Acceso a repositorios específicos
        IClienteRepository Clientes { get; }
        IProductoRepository Productos { get; }
        IVentaRepository Ventas { get; }
        IInventarioRepository Inventario { get; }

        // Estado de la transacción
        bool HasActiveTransaction { get; }

        // Métodos de guardado
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }

    // Interfaces específicas para cada repositorio (excepto IClienteRepository que ya existe en su propio archivo)
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetProductosBajosDeStockAsync(int stockMinimo);
        Task ActualizarStockAsync(string codigo, int unidades, double kilos);
        Task<bool> ValidarStockDisponibleAsync(string codigo, int unidades, double kilos);
    }

    public interface IVentaRepository : IRepository<Venta>
    {
        Task<IEnumerable<Venta>> GetVentasPorClienteAsync(string rut);
        Task<IEnumerable<Venta>> GetVentasEnRangoAsync(DateTime inicio, DateTime fin);
        Task<double> CalcularTotalVentasAsync(DateTime inicio, DateTime fin);
    }

    public interface IInventarioRepository : IRepository<Inventario>
    {
        Task<IEnumerable<Inventario>> GetInventarioPorFechaAsync(DateTime fecha);
        Task ActualizarInventarioAsync(string codigo, int unidades, double kilos);
        Task<IEnumerable<Inventario>> GetInventarioProximoAVencerAsync(int diasLimite);
    }
}