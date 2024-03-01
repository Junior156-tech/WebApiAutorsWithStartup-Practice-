using WebApiAutoresConStartup;

var builder = WebApplication.CreateBuilder(args);


var startup = new Startup(builder.Configuration);


//Services
startup.ConfigureServices(builder.Services);

var app = builder.Build();

var servicioLogger = (ILogger<Startup>)app.Services.GetService(typeof(ILogger<Startup>));
//Middleware
startup.Configure(app, app.Environment, servicioLogger);


app.Run();
