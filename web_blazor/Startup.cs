using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using web_blazor.Data;
using web_blazor.Services;

namespace web_blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
            //Thư viện auto mapper
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<IMapper, Mapper>();
            //http client để gọi api
            services.AddHttpClient();
            //Khai báo service (DI)
            services.AddScoped<CountService>();
            services.AddScoped<CryptoService>();
            services.AddScoped<ProductService>();
            services.AddScoped<RoomService>();
            //add service signalr
            services.AddSignalR();
            //Định nghĩa token jwt
            var privateKey = Configuration["jwt:private-key"];
            var Issuer = Configuration["jwt:Issuer"];
            var Audience = Configuration["jwt:Audience"];
            services.AddAuthentication("Bearer").AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Issuer,
                    ValidateAudience = true,
                    ValidAudience = Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
                    ClockSkew = TimeSpan.Zero,
                    RoleClaimType = ClaimTypes.Role, 
                    NameClaimType = ClaimTypes.Name,
                    //Verify thời gian token
                    ValidateLifetime = true
                };
            });
            //Add service jwt token
            services.AddScoped<JwtAuthService>();
            //  Đăng ký AuthenticationStateProvider tùy chỉnh
            services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            //Sử dụng blazor storage
            services.AddBlazoredLocalStorage();

            

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //Xác thực(Authentication) và phân quyền (Authorization)
            app.UseAuthentication(); //xác thực
            app.UseRouting();

            app.UseAuthorization();  //check role phân quyền


            //setup đường dẫn api hub cho ứng dụng
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<RoomHubs>("/room-hub");
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
