using CorePress.WebApi.Init;

var builder = WebApplication.CreateBuilder(args);

new WebApplicationInit(builder)
    .InitLogger()
    .InitConfig()
    .InitDataBase()
    .InitService()
    .CreateWebAppServer();

    