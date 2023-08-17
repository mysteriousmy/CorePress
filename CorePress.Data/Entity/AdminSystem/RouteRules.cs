using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity.AdminSystem;
/// <summary>
/// 路由
/// </summary>
[Table(Name = "cps_admin_route_rules")]
public class RouteRules : BaseAttr
{
    [Column(IsPrimary = true)]
    public int Id { get; set; }
    public int ParentId { get; set; }
    public string? RouteName { get; set; }
    public string? RouteUrl { get; set; }
    public string? Icon { get; set; }
    public string? Remark { get; set; }
    public bool? IsOperation { get; set; }
}