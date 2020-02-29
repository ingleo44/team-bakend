using System;
using System.IO;
using System.Reflection;
using System.Text;
using Blogs.Business.Classes;
using Blogs.Business.Interfaces;
using Blogs.Repository;
using Blogs.Repository.Classes;
using Blogs.Repository.Entities;
using Blogs.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace WebApi
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
            services.AddHttpContextAccessor();
            services.AddScoped<IUserSupervisor, UserSupervisor>();
            services.AddScoped<IAuthentication, Authentication>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBlogSupervisor, BlogSupervisor>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddDbContext<BlogsDbContext>(options => options.UseInMemoryDatabase(databaseName: "Blogsdb"));
            services.ConfigureSwaggerGen(options =>
            {
                options.IncludeXmlComments(Path.ChangeExtension(Assembly.GetEntryAssembly()?.Location, "xml"));
                
                //options.OperationFilter<AuthorizationHeaderParameterOperationFilter>();
                //options.OperationFilter<ODataParametersSwaggerDefinition>();

            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Description = "Api for retrieving information from the openreferrals system", Title = "Openreferrals API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}

                    }
                });
            });

            var jwtSection = Configuration.GetSection("jwt");
            var jwtOptions = new JwtOptions();
            jwtSection.Bind(jwtOptions);
            services.Configure<JwtOptions>(jwtSection);
            var key = Encoding.ASCII.GetBytes(jwtOptions.SecretKey);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
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
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Multitenant API V1");
                c.RoutePrefix = string.Empty;
            });
            AddTestData(app.ApplicationServices);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

     private void AddTestData(IServiceProvider sp)
        {
            var scopeServices = sp.CreateScope().ServiceProvider;
            var store = scopeServices.GetRequiredService<BlogsDbContext>();

            store.Users.Add(new User
            {
                Active = true, Id = -1, Username = "user1", Password = "password1", FirsName = "name1",
                LastName = "name1", CreationDate = DateTime.Now, DtLastUpdate = DateTime.Now
            });
            store.Users.Add(new User
            {
                Active = true,
                Id = -2,
                Username = "user2",
                Password = "password2",
                FirsName = "name2",
                LastName = "name2",
                CreationDate = DateTime.Now,
                DtLastUpdate = DateTime.Now
            });
            store.SaveChanges();
        }
    }
}
