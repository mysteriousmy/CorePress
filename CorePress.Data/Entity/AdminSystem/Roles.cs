using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;
/// <summary>
/// 角色
/// </summary>
[Table(Name = "cps_admin_roles")]
public class Roles : BaseAttr
{
    public int Id { get; set; }
    public string? RoleName { get; set; }
    public int SortNum { get; set; }
    public int Status { get; set; }
}