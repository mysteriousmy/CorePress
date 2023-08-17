using CorePress.WebApi.Init;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

new WebApplicationInit(builder)
    .InitLogger()
    .InitConfig()
    .InitDataBase()
    .InitService()
    .CreateWebAppServer();

    