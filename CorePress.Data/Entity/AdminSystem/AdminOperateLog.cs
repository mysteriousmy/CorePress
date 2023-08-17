using System.Collections.Concurrent;
using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;

/// <summary>
/// 操作日志
/// </summary>
[Table(Name = "cps_admin_operate_log")]
public class AdminOperateLog : BaseAttr
{
    // operatelog model properties
    [Column(IsPrimary = true)]
    public int Id { get; set; }
    
}