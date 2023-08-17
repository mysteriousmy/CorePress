using System.Collections.Generic;
using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;
/// <summary>
/// 系统字典和配置项
/// </summary>
[Table(Name = "cps_dic_config")]
public class SystemDicConfig : BaseAttr
{
    [Column(IsPrimary = true)]
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Remark { get; set; }
    public Dictionary<string, string>? Dict { get; set; }
}