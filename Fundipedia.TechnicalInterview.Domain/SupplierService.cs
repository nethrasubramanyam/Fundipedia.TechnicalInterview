using Fundipedia.TechnicalInterview.Data.Context;
using Fundipedia.TechnicalInterview.Model.Extensions;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fundipedia.TechnicalInterview.Domain;

public class SupplierService : ISupplierService
{
    private readonly SupplierContext _context;

    public SupplierService(SupplierContext context)
    {
        _context = context;
    }

    public async Task<Supplier> GetSupplier(Guid id)
    {
        var supplier = await _context.Suppliers.Include(x => x.Emails).Include(p => p.Phones).FirstOrDefaultAsync(s => s.Id == id); 
        return supplier;
    }

    public async Task<List<Supplier>> GetSuppliers()
    {
        return await _context.Suppliers.Include(x => x.Emails).Include(p => p.Phones).ToListAsync();
    }

    public async Task<Supplier> InsertSupplier(Supplier supplier)
    {
        if (!(supplier.ActivationDate >= DateTime.Today.AddDays(1)))
        {
            throw new Exception($"Invalid Activation date: {supplier.ActivationDate}");
        }
        if (supplier.IsValidEmail(out var invalidEmail) is false)
        {
            throw new Exception($"Invalid email address: {invalidEmail}");
        }
        if (supplier.IsValidPhoneNumber(out var invalidPhoneNumber) is false)
        {
            throw new Exception($"Invalid phone number: {invalidPhoneNumber}");
        }
        _context.Suppliers.Add(supplier);
        await _context.SaveChangesAsync();
        return supplier;

    }

    public async Task<Supplier> DeleteSupplier(Guid id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);
        if (supplier != null)
        {
            if (supplier.IsActive())
            {
                throw new Exception($"Supplier {id} is active, can't be deleted");
            }

            _context.Suppliers.Remove(supplier);
        }

        return supplier;
    }
}