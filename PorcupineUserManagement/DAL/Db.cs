using Microsoft.EntityFrameworkCore;
using PorcupineUserManagement.Models;

namespace PorcupineUserManagement.DAL;

public sealed class Db : DbContext
{
    public Db() : base() { }
    public Db(DbContextOptions<Db> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<GroupUser> GroupUsers { get; set; }
    public DbSet<GroupPermission> GroupPermissions { get; set; }
}