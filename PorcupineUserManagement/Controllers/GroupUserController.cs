using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;

namespace PorcupineUserManagement.Controllers;

/// <summary>
/// Provides API endpoints for managing <see cref="GroupUser"/> entities and their associated users and permissions.
/// </summary>
/// <remarks>
/// Inherits from <see cref="EntityController{GroupUser}"/> to provide base CRUD operations.
/// </remarks>
public class GroupUserController(Db db) : EntityController<GroupUser>(db);