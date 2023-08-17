using System;
using System.Net;
using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.UserSystem;
/// <summary>
/// 用户信息实体
/// </summary>
[Table(Name = "cps_user_info")]
public class UserInfo : BaseAttr
{
    [Column(IsPrimary = true)]
    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? NickName { get; set; }
    public string? Avatar { get; set; }
    public string? Pwd { get; set; }
    public IPAddress? RegisterIp { get; set; }
    public IPAddress? LastLoginIp { get; set; }
}