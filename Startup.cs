using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiNetCore.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Text;
using Azure.Storage.Blobs;
using ApiNetCore.Services;
using WkHtmlToPdfDotNet;
using WkHtmlToPdfDotNet.Contracts;

namespace api_netcore
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
            services.AddCors();
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

            var urlmongo = Configuration.GetValue<string>("mongourl");
            var database = Configuration.GetValue<string>("database");
            var secretkey = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("secretkey"));
            var blobconnection = Configuration.GetValue<string>("AzureBlobStorageConnectionString");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretkey),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddSingleton(serviceProvider =>
            {
                var mongoClient = new MongoClient(urlmongo);
                return mongoClient.GetDatabase(database);
            });

            services.AddSingleton(x => new BlobServiceClient(blobconnection));

            services.AddSingleton<IUsuariosRepository, UsuariosRepository>();
            services.AddSingleton<IEmpresasRepository, EmpresasRepository>();
            services.AddSingleton<ITiposFormaPagoRepository, TiposFormaPagoRepository>();
            services.AddSingleton<ITiposIdentificacionRepository, TiposIdentificacionRepository>();
            services.AddSingleton<ITipoProductoRepository, TipoProductoRepository>();
            services.AddSingleton<ITarifasIvaRepository, TarifasIvaRepository>();
            services.AddSingleton<IProductosRepository, ProductosRepository>();
            services.AddSingleton<IClientesRepository, ClientesRepository>();
            services.AddSingleton<IFacturaEmitidaRepository, FacturaEmitidaRepository>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddControllers(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "api_netcore", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var hosts = Configuration.GetValue<string>("AllowedHosts");
            app.UseCors(options =>
            {
                options.WithOrigins(hosts);
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "api_netcore v1"));
            }

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
