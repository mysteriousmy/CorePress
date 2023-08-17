using System;

namespace CorePress.Common.Const;

public interface ISystemConst
{
    public static readonly string RunBasePath = AppContext.BaseDirectory;
    public static readonly string WorkspaceBasePath = Environment.CurrentDirectory;
}