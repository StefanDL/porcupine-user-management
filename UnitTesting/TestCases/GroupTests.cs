using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.Controllers;
using PorcupineUserManagement.DAL;
using PorcupineUserManagement.Models;

namespace UnitTesting.TestCases;

public class GroupTests
{
    private readonly DbContextOptions<Db> _dbOptions = new DbContextOptionsBuilder<Db>()
        .UseInMemoryDatabase("TestDb")
        .Options;

    private static async Task SeedUser(Db db, Guid id)
    {
        var user = new User
        {
            Id = id,
            Name = "Test",
            Email = "test@example.com"
        };

        var controller = new UserController(db);
        await controller.CreateOrUpdate(user);

    }
    private static async Task SeedGroup(Db db, Guid id)
    {
        var group = new Group()
        {
            Id = id,
            Name = "Test Group",
        };

        var controller = new GroupController(db);
        await controller.CreateOrUpdate(group);

    }
    private static async Task SeedPermission(Db db, Guid id)
    {
        var permission = new Permission()
        {
            Id = id,
            Name = "Test Permission",
        };

        var controller = new PermissionController(db);
        await controller.CreateOrUpdate(permission);

    }
    private static async Task SeedGroupUser(Db db, Guid groupId, Guid userId)
    {
        var controller = new GroupController(db);
        await controller.AddUserToGroup(groupId, userId);
    }
    private static async Task SeedGroupPermission(Db db, Guid groupId, Guid permissionId)
    {
        var controller = new GroupController(db);
        await controller.AddPermissionToGroup(groupId, permissionId);
    }

    [Fact]
    public async Task GetGroupUsers_ReturnsOk_WithUsers()
    {
        // Arrange
        await using var context = new Db(_dbOptions);
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await SeedUser(context, userId);
        await SeedGroup(context, groupId);
        await SeedGroupUser(context, groupId, userId);
        var controller = new GroupController(context);
        
        // Act
        var result = await controller.GetGroupUsers(groupId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Single(users);
    }

    [Fact]
    public async Task GetGroupUsers_ReturnsEmptyList_IfNone()
    {
        // Arrange
        await using var context = new Db(_dbOptions);
        var groupId = Guid.NewGuid();
        var controller = new GroupController(context);
        
        // Act
        var result = await controller.GetGroupUsers(groupId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
        Assert.Empty(users);
    }

    [Fact]
    public async Task GetGroupUserCount_ReturnsCount()
    {
        // Arrange
        await using var context = new Db(_dbOptions);
        var groupId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        await SeedGroup(context, groupId);
        await SeedUser(context, userId);
        await SeedGroupUser(context, groupId, userId);
        var controller = new GroupController(context);

        // Act
        var result = await controller.GetGroupUserCount(groupId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var count = Assert.IsType<int>(okResult.Value);
        Assert.Equal(1, count);
    }

    [Fact]
    public async Task GetGroupPermissions_ReturnsOk_WithPermissions()
    {
        // Arrange
        await using var context = new Db(_dbOptions);
        var groupId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        await SeedPermission(context, permissionId);
        await SeedGroup(context, groupId);
        await SeedGroupPermission(context, groupId, permissionId);
        var controller = new GroupController(context);

        // Act
        var result = await controller.GetGroupPermissions(groupId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var permissions = Assert.IsAssignableFrom<IEnumerable<Permission>>(okResult.Value);
        Assert.Single(permissions);
    }

    [Fact]
    public async Task GetGroupPermissions_ReturnsEmpty_OnError()
    {
        // Arrange
        await using var context = new Db(_dbOptions);
        var controller = new GroupController(context);

        // Act
        var result = await controller.GetGroupPermissions(Guid.NewGuid());

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var permissions = Assert.IsAssignableFrom<IEnumerable<Permission>>(okResult.Value);
        Assert.Empty(permissions);
    }

    [Fact]
    public async Task GetGroupPermissions_ReturnsListOfGroupPermission()
    {
        //Arrange
        await using var context = new Db(_dbOptions);
        var groupId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();
        await SeedGroup(context, groupId);
        await SeedPermission(context, permissionId);
        await SeedGroupPermission(context, groupId, permissionId);
        var controller = new GroupController(context);
        
        //Act
        var result = await controller.GetGroupPermissions(groupId);
        
        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var permissions = Assert.IsAssignableFrom<IEnumerable<Permission>>(okResult.Value);
        Assert.Contains(permissionId, permissions.Select(x => x.Id));
    }

}