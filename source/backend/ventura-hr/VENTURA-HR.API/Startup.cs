using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VENTURA_HR.DOMAIN.UsuarioAggregate.Repositories;
using VENTURA_HR.DOMAIN.VagaAggregate.Services;
using VENTURA_HR.Services;
using VENTURA_HT.Repository.Context;
using VENTURA_HT.Repository.Repositories;

namespace VENTURA_HR.API
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
			services.AddScoped<AuthService>();
			services.AddScoped<IUsuarioService, UsuarioService>();
			services.AddScoped<IUsuarioRepository, UsuarioRepository>();
			services.AddScoped<ICandidatoRepository, CandidatoRepository>();
			services.AddScoped<IEmpresaRepository, EmpresaRepository>();
			services.AddScoped<IAdministradorRepository, AdministradorRepository>();

			services.AddDbContext<VenturaContext>(opt => opt.UseSqlServer(Configuration.GetConnectionString("ConnStr")));

			services.AddCors();
			services.AddControllers();

			var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("secret"));
			//Token config schema (Bearer)
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			//Token: Tipo e Config
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				// Precisa validar a chave (simetrica)
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					// N�o vamos usar uma aplica�ao distribuida pra outras.
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
				.AllowAnyOrigin());

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}