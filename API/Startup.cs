using Application.Activities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;

namespace API
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
            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });
            //VON PAKKO________________________________
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy",policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000");
                });
            });

            services.AddMediatR(typeof(List.Handler).Assembly);
            //Alternative von : https://dotnetcoretutorials.com/2019/04/30/the-mediator-pattern-part-3-mediatr-library/
            // kann man vlt. auch probieren (ist generischer):
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            //*******************************************
            services.AddControllers();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //PAKO: wurde manuell auskommentiert, wird am Ende wieder einkommentiert. (Security)
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            //VON PAKO:
            app.UseCors("CorsPolicy");
//********************************************
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            

        }
    }
}
