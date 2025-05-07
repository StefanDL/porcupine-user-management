using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;
using Serilog;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// Handles HTTP requests related to the <see cref="User"/> entity, including user-specific associations.
/// </summary>
/// <remarks>
/// Extends <see cref="EntityController{User}"/> to provide endpoints for CRUD operations.
/// </remarks>
public class UserController(Db db) : EntityController<User>(db)
{
    /// <summary>
    /// Gets the list of groups associated with a specific user.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the user's groups if found;
    /// otherwise, returns <see cref="NoContentResult"/> or <see cref="BadRequestResult"/>.
    /// </returns>
    [HttpGet("{id:guid}/groups")]
    public async Task<IActionResult> GetUserGroups(Guid id)
    {
        try
        {
            var user = await Db.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null) return NoContent();
            var groups = await (
                from groupUser in Db.GroupUsers
                join groupEntity in Db.Groups on groupUser.GroupId equals groupEntity.Id
                where groupUser.UserId == id && !groupEntity.IsDeleted
                select groupEntity).AsNoTracking().ToListAsync();
            return Ok(groups);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
        
    }

    /// <summary>
    /// Gets the list of permissions assigned to a user via group membership.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> with the permissions list, or <see cref="NoContentResult"/> if the user is not found,
    /// or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpGet("{id:guid}/permissions")]
    public async Task<IActionResult> GetUserPermissions(Guid id)
    {
        try
        {
            var user = await Db.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            if (user == null) return NoContent();
            var permissions = await (from groupUser in Db.GroupUsers
                join groupPermission in Db.GroupPermissions on groupUser.GroupId equals groupPermission.GroupId
                where groupUser.UserId == id && !groupPermission.IsDeleted
                select groupPermission).AsNoTracking().ToListAsync();
            return Ok(permissions);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
        
    }
}