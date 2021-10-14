using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Common;
using eShopSolution.Application.System.Languages;
using eShopSolution.Application.System.Roles;
using eShopSolution.Application.System.Users;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using eShopSolution.ViewModels.System.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace eShopSolution.BackendApi
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
            //Dang ky database
            services.AddDbContext<eShopDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

            //Dang ky service cho Identity
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<eShopDbContext>()
                .AddDefaultTokenProviders();

            //Declare DI - Dang ky dich vu cho cac API

            services.AddTransient<IProductService, ProductService>();//Mỗi lần request thì khoi tao IProductService và chỉ ra class hiện thực là ProductService truoc khi tiem inject vao ProductController
            services.AddTransient<IStorageService, FileStorageService>();

            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<IRoleService, RoleService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILanguageService, LanguageService>();
            services.AddTransient<ICategoryService, CategoryService>();

            // Dang ky dich vu cho các Validator

            //services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();// Dang ky service cho Validator, In order for ASP.NET to discover your validators, they must be registered with the services collection. You can either do this by calling the AddTransient method for each of your validators
            //services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>(); //Ko cần nữa! do có RegisterValidatorsFromAssemblyContaining rồi (cả đám cùng .dll rồi)

            services.AddControllers().AddFluentValidation( //swagger nên ko cần WithView //AddFluentValidation để bắt Validation các ModelView
                fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>()); //Dang ky tat ca cac Validator cung Project voi LoginRequestValidator: o day la eShopSolution.ViewModels

            // Dang ky dich vu cho Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo //option chỉ ra tên + version của swagger
                {
                    Title = "Swagger eShopSolution",
                    Version = "v1",
                    Description = "Test cac services",
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme //Thêm nút Authorize ra màn hình Swagger
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                     }
                });
            });

            //Add service JwtBearer Authentication, khi xac thuc khong duoc thi cac phuong thuc tra ve loi 401
            string issuer = Configuration.GetValue<string>("Tokens:Issuer");
            string signingKey = Configuration.GetValue<string>("Tokens:Key");
            byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(opt =>
            {
                //set the default to authenticate and challenge schemes to Bearer in this application
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>        //configure the JWT Bearer token, especially the token validation parameters
            {
                options.RequireHttpsMetadata = false; //default value is true, which means that the authentication requires HTTPS for the metadata address or authority
                options.SaveToken = true; //saves the JWT access token in the current HttpContext, so that we can retrieve it using the method await HttpContext.GetTokenAsync(“Bearer”, “access_token”)
                options.TokenValidationParameters = new TokenValidationParameters() // Sử dụng những parameters này để validate Token nhận được, this object sets the parameters used to validate identity tokens. The meaning for each property is self-explanatory. One thing I want to mention is the ClockSkew property. I set its value to be one minute, which gives an allowance time for the token expiration validation. I have an integration test for this property, and you can play with it.
                {
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = issuer,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,//TimeSpan.FromMinutes(10), //I set its value to be one minute, which gives an allowance time for the token expiration validation.
                    IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
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
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication(); //1. Xac thuc: registered authentication schemes (JWT Bearer, in this case) work
            app.UseRouting(); //2. Routing

            app.UseAuthorization(); //3. Phân quyền

            app.UseSwagger();

            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}