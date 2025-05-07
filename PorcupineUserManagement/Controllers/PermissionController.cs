using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// Provides API endpoints for managing <see cref="Permission"/> entities and their associated users and permissions.
/// </summary>
/// <remarks>
/// Inherits from <see cref="EntityController{Permission}"/> to provide base CRUD operations.
/// </remarks>
public class PermissionController(Db db) : EntityController<Permission>(db);