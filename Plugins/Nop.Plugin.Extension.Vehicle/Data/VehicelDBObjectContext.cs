using Nop.Core;
using Nop.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Extension.Vehicle.Data
{
    public class VehicelDBObjectContext :DbContext, IDbContext
    {
        public bool ProxyCreationEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool AutoDetectChangesEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDbContext.ProxyCreationEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDbContext.AutoDetectChangesEnabled
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public VehicelDBObjectContext(string nameOrConnectionString) : base(nameOrConnectionString) { }

        #region Implementation of IDbContext

        #endregion

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Configurations.Add(new MakeMap());
            modelBuilder.Configurations.Add(new VehicleTypeGroupMap());
            modelBuilder.Configurations.Add(new VehicleTypeMap());
            
            modelBuilder.Configurations.Add(new VehicleModelMap());
            modelBuilder.Configurations.Add(new BaseVehicleMap());
            modelBuilder.Configurations.Add(new VehicleRecordMap());
            modelBuilder.Configurations.Add(new EngineMap());
            modelBuilder.Configurations.Add(new VehicleProductMap());

            base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseInstallationScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public void Install()
        {
            //It's required to set initializer to null (for SQL Server Compact).
            //otherwise, you'll get something like "The model backing the 'your context name' context has changed since the database was created. Consider using Code First Migrations to update the database"
            Database.SetInitializer<VehicelDBObjectContext>(null);

            Database.ExecuteSqlCommand(CreateDatabaseInstallationScript());
            SaveChanges();
        }

        //public IList<TEntity> ExecuteMyProcedureList<TEntity>(string commandText, params object[] parameters)
        //{
        //    return ExecuteStoredProcedureList(commandText, parameters);
        //}
        public void Uninstall()
        {
            //var dbScript = "DROP TABLE ProductViewTracking";
            //Database.ExecuteSqlCommand(dbScript);
            //SaveChanges();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        IDbSet<TEntity> IDbContext.Set<TEntity>()
        {
            throw new NotImplementedException();
        }

        int IDbContext.SaveChanges()
        {
           
            throw new NotImplementedException();
        }

         IList<TEntity> IDbContext.ExecuteStoredProcedureList<TEntity>(string commandText, params object[] parameters)
        {
          
            throw new NotImplementedException();
        }

        IEnumerable<TElement> IDbContext.SqlQuery<TElement>(string sql, params object[] parameters)
        {
            return this.Database.SqlQuery<TElement>(sql, parameters);
        }

        int IDbContext.ExecuteSqlCommand(string sql, bool doNotEnsureTransaction, int? timeout, params object[] parameters)
        {
            int? previousTimeout = null;
            if (timeout.HasValue)
            {
                //store previous timeout
                previousTimeout = ((IObjectContextAdapter)this).ObjectContext.CommandTimeout;
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = timeout;
            }

            var transactionalBehavior = doNotEnsureTransaction
                ? TransactionalBehavior.DoNotEnsureTransaction
                : TransactionalBehavior.EnsureTransaction;
            var result = this.Database.ExecuteSqlCommand(transactionalBehavior, sql, parameters);

            if (timeout.HasValue)
            {
                //Set previous timeout back
                ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = previousTimeout;
            }

            //return result
            return result;
        }

        void IDbContext.Detach(object entity)
        {
            throw new NotImplementedException();
        }
    }

    
}
