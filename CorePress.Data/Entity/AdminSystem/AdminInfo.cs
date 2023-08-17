using System;
using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;
/// <summary>
/// 管理员信息实体
/// </summary>
[Table(Name = "cps_admin_info")]
public class AdminInfo : BaseAttr
{
    [Column(IsPrimary = true)]
    public Guid Id { get; set; }
    public string? AdminPhone { get; set; }
    public string? AdminName { get; set; }
    public string? AdminPwd { get; set; }
    public int AdminRole { get; set; }
}