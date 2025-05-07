using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.Controllers;
using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;

namespace UnitTesting.TestCases;

public class UserTests
{
    private readonly DbContextOptions<Db> _dbOptions = new DbContextOptionsBuilder<Db>()
        .UseInMemoryDatabase("TestDb")
        .Options;
    
    private static async Task SeedTestData(Db db, Guid userId, Guid groupId, Guid permissionId)
    {
        var user = new User { Id = userId, Name = "John", Email = "test@example.com" };
        var group = new Group { Id = groupId, Name = "Admins" };
        var groupUser = new GroupUser { UserId = userId, GroupId = groupId };
        var permission = new Permission { Id = permissionId, Name = "Edit" };
        var groupPermission = new GroupPermission { GroupId = groupId, PermissionId = permissionId };

        var userController = new UserController(db);
        await userController.CreateOrUpdate(user);

        var groupController = new GroupController(db);
        await groupController.CreateOrUpdate(group);

        var groupUserController = new GroupUserController(db);
        await groupUserController.CreateOrUpdate(groupUser);

        var permissionController = new PermissionController(db);
        await permissionController.CreateOrUpdate(permission);

        var groupPermissionController = new GroupPermissionController(db);
        await groupPermissionController.CreateOrUpdate(groupPermission);
    }

    [Fact]
    public async Task GetUserGroups_ReturnsGroups()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();

        await using var db = new Db(_dbOptions);
        await SeedTestData(db, userId, groupId, Guid.NewGuid());
        var controller = new UserController(db);

        // Act
        var result = await controller.GetUserGroups(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var groups = Assert.IsAssignableFrom<List<Group>>(okResult.Value);
        Assert.Single(groups);
        Assert.Equal(groupId, groups.First().Id);
    }

    [Fact]
    public async Task GetUserPermissions_ReturnsPermissions()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var groupId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        await using var db = new Db(_dbOptions);
        await SeedTestData(db, userId, groupId, permissionId);
        var controller = new UserController(db);

        // Act
        var result = await controller.GetUserPermissions(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var permissions = Assert.IsAssignableFrom<List<Permission>>(okResult.Value);
        Assert.Single(permissions);
        Assert.Equal(permissionId, permissions.First().Id);
    }
}