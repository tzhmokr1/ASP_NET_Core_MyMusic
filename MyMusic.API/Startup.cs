using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using MyMusic.Core;
using MyMusic.Core.Repositories;
using MyMusic.Core.Services;
using MyMusic.Data;
using MyMusic.Data.MongoDB.Repository;
using MyMusic.Data.MongoDB.Setting;
using MyMusic.Services.Services;
using System.Text;
using System.Threading.Tasks;

namespace MyMusic.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Configuration for SQL server
            services.AddDbContext<MyMusicDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default"), x => x.MigrationsAssembly("MyMusic.Data")));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMusicService, MusicService>();
            services.AddTransient<IArtistService, ArtistService>();
            services.AddTransient<IUserService, UserService>();

            // Configuration for MongoDB
            services.Configure<Settings>(
           options =>
           {
               options.ConnectionString = Configuration.GetValue<string>("MongoDB:ConnectionString");
               options.Database = Configuration.GetValue<string>("MongoDB:Database");
           });

            services.AddSingleton<IMongoClient, MongoClient>(
            _ => new MongoClient(Configuration.GetValue<string>("MongoDB:ConnectionString")));

            services.AddTransient<IDatabaseSettings, DatabaseSettings>();
            services.AddScoped<IComposerRepository, ComposerRepository>();
            services.AddTransient<IComposerService, ComposerService>();
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Put title here",
                    Description = "DotNet Core Api 3 - with swagger"
                });
            });

            //Automapper
            services.AddAutoMapper(typeof(Startup));

            //Authentication 
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("AppSettings:Secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(x =>
                 {
                     x.Events = new JwtBearerEvents
                     {
                         OnTokenValidated = context =>
                         {
                             var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                             var userId = int.Parse(context.Principal.Identity.Name);
                             var user = userService.GetById(userId);
                             if (user == null)
                             {
                                 // return unauthorized if user no longer exists
                                 context.Fail("Unauthorized");
                             }
                             return Task.CompletedTask;
                         }
                     };
                     x.RequireHttpsMetadata = false;
                     x.SaveToken = true;
                     x.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(key),
                         ValidateIssuer = false,
                         ValidateAudience = false
                     };
                 });



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "";
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Music V1");
            });
        }
    }
}
