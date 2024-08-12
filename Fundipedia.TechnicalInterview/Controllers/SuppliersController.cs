using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Fundipedia.TechnicalInterview.Model.Supplier;
using Fundipedia.TechnicalInterview.Domain;

namespace Fundipedia.TechnicalInterview.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    // GET: api/Suppliers
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> GetSupplier()
    {
        return await _supplierService.GetSuppliers();
    }

    // GET: api/Suppliers/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> GetSupplier(Guid id)
    {
        var supplier = await _supplierService.GetSupplier(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return supplier;
    }

    // POST: api/Suppliers
    [HttpPost]
    public async Task<ActionResult<Supplier>> PostSupplier(Supplier supplier)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var response  = await _supplierService.InsertSupplier(supplier);


        return CreatedAtAction("GetSupplier", new { id = supplier.Id }, supplier);
    }

    // DELETE: api/Suppliers/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<Supplier>> DeleteSupplier(Guid id)
    {
        var supplier = await _supplierService.DeleteSupplier(id);
        return supplier;
    }
}