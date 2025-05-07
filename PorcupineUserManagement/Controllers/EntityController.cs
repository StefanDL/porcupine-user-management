using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;
using Serilog;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// A generic API controller for managing CRUD operations on entities.
/// </summary>
/// <typeparam name="T">The entity type that inherits from BaseDbEntity.</typeparam>
[ApiController]
[Route("api/[controller]")]
public class EntityController<T>(Db db) : Controller where T : BaseDbEntity
{
    /// <summary>
    /// The database context used for accessing entity data.
    /// </summary>
    protected readonly Db Db = db;

    /// <summary>
    /// Retrieves an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>The entity if found, or appropriate HTTP status code.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var entity = await Db.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            return entity == null ? NoContent() : Ok(entity);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves the total count of non-deleted entities.
    /// </summary>
    /// <returns>The total count.</returns>
    [HttpGet("count")]
    public async Task<IActionResult> GetCount()
    {
        try
        {
            var total = await Db.Set<T>().AsNoTracking().CountAsync(x => !x.IsDeleted);
            return Ok(total);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a paginated list of entities.
    /// </summary>
    /// <param name="page">The page number (default is 1).</param>
    /// <param name="pageSize">The page size (default is 10).</param>
    /// <returns>Paginated list of entities and total count.</returns>
    [HttpGet("")]
    public async Task<IActionResult> GetList([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = Db.Set<T>().AsNoTracking().Where(x => !x.IsDeleted).OrderBy(x => x.Created);
            var total = await query.CountAsync();
            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return Ok(new { total, data, page, pageSize });
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Creates or updates an entity.
    /// </summary>
    /// <param name="entity">The entity to save.</param>
    /// <returns>The saved entity or validation errors.</returns>
    [HttpPost]
    public async Task<IActionResult> CreateOrUpdate([FromBody] T entity)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await Db.Set<T>().FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing == null)
            {
                entity.Created = DateTime.UtcNow;
                await Db.Set<T>().AddAsync(entity);
            }
            else
            {
                entity.Updated = DateTime.UtcNow;
                Db.Entry(existing).CurrentValues.SetValues(entity);
            }

            await Db.SaveChangesAsync();
            return Ok(entity);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Marks an entity as deleted.
    /// </summary>
    /// <param name="id">The ID of the entity to delete.</param>
    /// <returns>The updated entity or appropriate HTTP status code.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var entity = await Db.Set<T>().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (entity == null) return NoContent();

            entity.IsDeleted = true;
            await Db.SaveChangesAsync();
            return Ok(entity);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }
}