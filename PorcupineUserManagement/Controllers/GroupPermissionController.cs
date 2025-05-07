using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// Provides API endpoints for managing <see cref="GroupPermission"/> entities and their associated users and permissions.
/// </summary>
/// <remarks>
/// Inherits from <see cref="EntityController{GroupPermission}"/> to provide base CRUD operations.
/// </remarks>
public class GroupPermissionController(Db db) : EntityController<GroupPermission>(db);