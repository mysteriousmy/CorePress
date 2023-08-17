using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;
/// <summary>
/// 管理员角色路由权限实体
/// </summary>
[Table(Name = "cps_admin_role_rules")]
public class AdminRoleRules
{
    [Column(IsPrimary = true)]
    public int Id { get; set; }
    public int RuleId { get; set; }
    public int RoleId { get; set; }
    public bool? IsDeleted { get; set; }
}