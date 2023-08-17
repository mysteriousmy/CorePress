
using System;
using FreeSql.DataAnnotations;

namespace CorePress.Data.Entity;


public class BaseAttr
{
    [Column(ServerTime = DateTimeKind.Local, CanUpdate = false)]
    public DateTime CreateTime { get; set; }
    [Column(ServerTime = DateTimeKind.Local)]
    public DateTime UpdateTime { get; set; }
    public int IsDeleted { get; set; }
}