using System;
using System.IO;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CorePress.Common.Const;
using CorePress.Common.Utils;
using CorePress.Data.Entity.AdminSystem;
using CorePress.Data.Entity.UserSystem;
using FreeSql;
using FreeSql.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace CorePress.WebApi.Init;

public sealed class WebApplicationInit
{
    private WebApplicationBuilder _webApplicationBuilder;
    private string? _localDataBaseUrl;
    private bool _dataBaseLog;
    private IFreeSql? _localDb;
    // private string _remoteDataBaseUrl;
    private string? _aesKey;
    private string? _aesKeyIv;
    
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="webApplicationBuilder"></param>
    public WebApplicationInit(WebApplicationBuilder webApplicationBuilder)
    {
        _webApplicationBuilder = webApplicationBuilder;
    }
    
    /// <summary>
    /// 初始化SerialLog日志中间件
    /// </summary>
    /// <returns></returns>
    public WebApplicationInit InitLogger()
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
            .CreateBootstrapLogger();
        var text = File.ReadAllText(@$"{ISystemConst.RunBasePath}/banner.txt");
        Log.Information("Starting UCG-System Web Application");
        Console.WriteLine(text);
        _webApplicationBuilder.Host.UseSerilog((context, services, configuration) => configuration
            // .MinimumLevel.Debug()
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .ReadFrom.Services(services));
        return this;
    }

    /// <summary>
    /// 初始化配置
    /// </summary>
    /// <returns></returns>
    public WebApplicationInit InitConfig()
    {
        _localDataBaseUrl = _webApplicationBuilder.Configuration["DataBaseConfig:PgSqlLocal:url"];
        _aesKey = _webApplicationBuilder.Configuration["KeySalt:AesKey"];
        _aesKeyIv = _webApplicationBuilder.Configuration["KeySalt:AesKeyIv"];
        _dataBaseLog = Convert.ToBoolean(_webApplicationBuilder.Configuration["DataBaseConfig:PgSqlLocal:log"]);
        return this;
    }
    
    /// <summary>
    /// 初始化数据库
    /// </summary>
    /// <returns></returns>
    public WebApplicationInit InitDataBase()
    {
        _localDb = new FreeSqlBuilder()
                .UseConnectionString(DataType.PostgreSQL, _localDataBaseUrl)
                .UseMonitorCommand(cmd =>
                {
                    if (_dataBaseLog) Log.Information($"本地数据库执行Sql：{cmd.CommandText}");
                })//监听SQL语句
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                .Build();
        return this;
    }
    
    /// <summary>
    /// 初始化Service
    /// </summary>
    /// <returns></returns>
    public WebApplicationInit InitService()
    {
        _webApplicationBuilder.Services.AddSingleton(_localDb);
        _webApplicationBuilder.Services.AddControllers();
        _webApplicationBuilder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        _webApplicationBuilder.Host.ConfigureContainer<ContainerBuilder>((_, containerBuilder) =>
        {
            //containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Ser")).AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterAssemblyTypes(GetAssembly("Ucg.Sys.Service")).AsImplementedInterfaces().InstancePerLifetimeScope();
            containerBuilder.RegisterType<AutoFacUtil>().WithParameter((pi, _) => pi.ParameterType == typeof(IServiceProvider), (_, ctx) => ctx.Resolve<IServiceProvider>()).InstancePerLifetimeScope();
            //containerBuilder.RegisterType<LoggingAttribute>().InstancePerLifetimeScope();
            // containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith(“Repository”)).AsImplementedInterfaces().InstancePerLifetimeScope();
        });
        _webApplicationBuilder.Services.AddEndpointsApiExplorer();
        _webApplicationBuilder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Version = "v1",
                Title = "CorePress接口文档",
                Description = "CorePress博客系统文档"
            });
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        }); 
        // _webApplicationBuilder.Services.AddMvc().AddMvcOptions(option =>
        // {
        //     option.OutputFormatters.Clear();
        //     option.OutputFormatters.Add(new MessagePackOutputFormatter(ContractlessStandardResolver.Options));
        //     option.InputFormatters.Clear();
        //     option.InputFormatters.Add(new MessagePackInputFormatter(ContractlessStandardResolver.Options));
        // });
        return this;
    }

    /// <summary>
    /// 建立初始化webserver
    /// </summary>
    public void CreateWebAppServer()
    {
        var app = _webApplicationBuilder.Build();
        
        using (var serviceScope = app.Services.CreateScope())
        {
            var fsql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
            fsql.CodeFirst.IsAutoSyncStructure = true;
            fsql.CodeFirst.SyncStructure(typeof(AdminInfo));
            fsql.CodeFirst.SyncStructure(typeof(UserInfo));
            fsql.CodeFirst.SyncStructure(typeof(Roles));
            // fsql.Aop.CurdAfter += (s, e) =>
            // {
            //     Console.WriteLine($"ManagedThreadId:{Thread.CurrentThread.ManagedThreadId};"+
            //                       $" FullName:{e.EntityType.FullName} ElapsedMilliseconds:{e.ElapsedMilliseconds}ms, {e.Sql}");
            //     if (e.ElapsedMilliseconds > 200)
            //     {
            //         //记录日志
            //         //发送短信给负责人
            //     }
            // };
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web App V1");
                c.RoutePrefix = "swagger";
            });
        }
        app.UseFileServer(new FileServerOptions {
            RequestPath = "",
            FileProvider = new Microsoft.Extensions.FileProviders
                .ManifestEmbeddedFileProvider(
                    typeof(Program).Assembly, "front/out"
                ) 
        });
        
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="assemblyName"></param>
    /// <returns></returns>
    public static Assembly GetAssembly(string assemblyName)
    {
        // var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(AppContext.BaseDirectory + $"{assemblyName}.dll");
        var assembly = Assembly.Load(assemblyName);
        return assembly;
    }
}