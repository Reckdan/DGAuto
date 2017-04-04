using Autofac;
using Autofac.Core;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using Nop.Data;
using Nop.Plugin.Extension.Vehicle.Data;
using Nop.Plugin.Extension.Vehicle.Domain;
using Nop.Plugin.Extension.Vehicle.Services;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Extension.Vehicle.Infrastructure
{
    public class DependencyRegistrar:IDependencyRegistrar
    {
        private const string CONTEXT_NAME = "nop_object_context_vehicles";

        public virtual void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<VehicleRecordService>().As<IVehicleRecordService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<VehicelDBObjectContext>(builder, CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<VehicleRecord>>()
                .As<IRepository<VehicleRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            
            builder.RegisterType<EfRepository<VehicleTypeGroup>>()
                .As<IRepository<VehicleTypeGroup>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleType>>()
                .As<IRepository<VehicleType>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<Make>>()
                .As<IRepository<Make>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleModel>>()
                .As<IRepository<VehicleModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<BaseVehicle>>()
                .As<IRepository<BaseVehicle>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<Engine>>()
                .As<IRepository<Engine>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleProduct>>()
                .As<IRepository<VehicleProduct>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<VehicleRecordService>().As<IVehicleRecordService>().InstancePerLifetimeScope();

            //data context
            this.RegisterPluginDataContext<VehicelDBObjectContext>(builder, CONTEXT_NAME);

            //override required repository with our custom context
            builder.RegisterType<EfRepository<VehicleRecord>>()
                .As<IRepository<VehicleRecord>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleTypeGroup>>()
                .As<IRepository<VehicleTypeGroup>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleType>>()
                .As<IRepository<VehicleType>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<Make>>()
                .As<IRepository<Make>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleModel>>()
                .As<IRepository<VehicleModel>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<BaseVehicle>>()
                .As<IRepository<BaseVehicle>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<Engine>>()
                .As<IRepository<Engine>>()
                .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
                .InstancePerLifetimeScope();
            builder.RegisterType<EfRepository<VehicleProduct>>()
               .As<IRepository<VehicleProduct>>()
               .WithParameter(ResolvedParameter.ForNamed<IDbContext>(CONTEXT_NAME))
               .InstancePerLifetimeScope();
        }

        public int Order
        {
            get { return 1; }
        }
    }
}
