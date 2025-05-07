using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;
using Serilog;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// Provides API endpoints for managing <see cref="Group"/> entities and their associated users and permissions.
/// </summary>
/// <remarks>
/// Inherits from <see cref="EntityController{Group}"/> to provide base CRUD operations.
/// </remarks>
public class GroupController(Db db) : EntityController<Group>(db)
{
    /// <summary>
    /// Gets the list of users in a specific group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> containing the users list or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpGet("{groupId:guid}/users")]
    public async Task<IActionResult> GetGroupUsers(Guid groupId)
    {
        try
        {
            var users = await (from groupUser in Db.GroupUsers
                join user in Db.Users on groupUser.UserId equals user.Id
                where groupUser.GroupId == groupId && !groupUser.IsDeleted
                select user).AsNoTracking().ToListAsync();
            return Ok(users);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the total number of users in a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> with the user count or <see cref="BadRequestResult"/> on error.
    /// </returns>
    [HttpGet("{groupId:guid}/users/count")]
    public async Task<IActionResult> GetGroupUserCount(Guid groupId)
    {
        try
        {
            var count = await (from groupUser in Db.GroupUsers
                join user in Db.Users on groupUser.UserId equals user.Id
                where groupUser.GroupId == groupId && !groupUser.IsDeleted
                select user).AsNoTracking().CountAsync();
            return Ok(count);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the list of permissions assigned to a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> with the list of permissions or <see cref="BadRequestResult"/> on error.
    /// </returns>
    [HttpGet("{groupId:guid}/permissions")]
    public async Task<IActionResult> GetGroupPermissions(Guid groupId)
    {
        try
        {
            var permissions = await (from groupPermission in Db.GroupPermissions
                join permission in Db.Permissions on groupPermission.PermissionId equals permission.Id
                where groupPermission.GroupId == groupId && !groupPermission.IsDeleted
                select permission).AsNoTracking().ToListAsync();
            return Ok(permissions);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Gets the count of permissions assigned to a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <returns>
    /// An <see cref="IActionResult"/> with the count of permissions or <see cref="BadRequestResult"/> on error.
    /// </returns>
    [HttpGet("{groupId:guid}/permissions/count")]
    public async Task<IActionResult> GetGroupPermissionsCount(Guid groupId)
    {
        try
        {
            var count = await (from groupPermission in Db.GroupPermissions
                join permission in Db.Permissions on groupPermission.PermissionId equals permission.Id
                where groupPermission.GroupId == groupId && !groupPermission.IsDeleted
                select permission).AsNoTracking().CountAsync();
            
            return Ok(count);
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Adds a user to a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to add.</param>
    /// <returns>
    /// <see cref="OkResult"/> if successful, <see cref="NoContentResult"/> if the group or user is not found,
    /// or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpPost("{groupId:guid}/add-user/{userId:guid}")]
    public async Task<IActionResult> AddUserToGroup(Guid groupId, Guid userId)
    {
        try
        {
            var group = await Db.Set<Group>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId && !x.IsDeleted);
            if (group == null) return NoContent();
            var user = await Db.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
            if (user == null) return NoContent();
            await Db.GroupUsers.AddAsync(new GroupUser { GroupId = groupId, UserId = userId });
            await Db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Removes a user from a group by marking the relationship as deleted.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="userId">The unique identifier of the user to remove.</param>
    /// <returns>
    /// <see cref="OkResult"/> if successful, <see cref="NoContentResult"/> if the group, user, or relation is not found,
    /// or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpDelete("{groupId:guid}/remove-user/{userId:guid}")]
    public async Task<IActionResult> RemoveUserFromGroup(Guid groupId, Guid userId)
    {
        try
        {
            var group = await Db.Set<Group>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId && !x.IsDeleted);
            if (group == null) return NoContent();
            var user = await Db.Set<User>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
            if (user == null) return NoContent();
            var groupUser = await Db.GroupUsers.FirstOrDefaultAsync(x => x.GroupId == groupId && x.UserId == userId);
            if (groupUser == null) return NoContent();
            groupUser.IsDeleted = true;
            await Db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Adds a permission to a group.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    /// <returns>
    /// <see cref="OkResult"/> if successful, <see cref="NoContentResult"/> if not found,
    /// or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpPost("{groupId:guid}/add-permission/{permissionId:guid}")]
    public async Task<IActionResult> AddPermissionToGroup(Guid groupId, Guid permissionId)
    {
        try
        {
            var group = await Db.Set<Group>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId && !x.IsDeleted);
            if (group == null) return NoContent();
            var permission = await Db.Set<Permission>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == permissionId && !x.IsDeleted);
            if (permission == null) return NoContent();
            await Db.GroupPermissions.AddAsync(new GroupPermission { GroupId = groupId, PermissionId = permissionId });
            await Db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }

    /// <summary>
    /// Removes a permission from a group by marking the relationship as deleted.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="permissionId">The unique identifier of the permission.</param>
    /// <returns>
    /// <see cref="OkResult"/> if successful, <see cref="NoContentResult"/> if not found,
    /// or <see cref="BadRequestResult"/> if an error occurs.
    /// </returns>
    [HttpDelete("{groupId:guid}/remove-permission/{permissionId:guid}")]
    public async Task<IActionResult> RemovePermissionFromGroup(Guid groupId, Guid permissionId)
    {
        try
        {
            var group = await Db.Set<Group>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == groupId && !x.IsDeleted);
            if (group == null) return NoContent();
            var permission = await Db.Set<Permission>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == permissionId && !x.IsDeleted);
            if (permission == null) return NoContent();
            var groupPermission = await Db.GroupPermissions.FirstOrDefaultAsync(x => x.GroupId == groupId && x.PermissionId == permissionId);
            if (groupPermission == null) return NoContent();
            groupPermission.IsDeleted = true;
            await Db.SaveChangesAsync();
            return Ok();
        }
        catch (Exception e)
        {
            Log.Error(e, "Exception");
            return BadRequest();
        }
    }
}