using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestNHibernatePostgreSQL.Entities;

namespace TestNHibernatePostgreSQL
{
    class Program
    {
        static void Main(string[] args)
        {
            // create our NHibernate session factory
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    var potatoes = new Product { Name = "Potatoes", Price = 3.60 };
                    var fish = new Product { Name = "鱼Fish", Price = 4.49 };
                    var milk = new Product { Name = "Milk", Price = 0.79 };
                    var bread = new Product { Name = "Bread", Price = 1.29 };
                    var cheese = new Product { Name = "Cheese", Price = 2.10 };
                    var waffles = new Product { Name = "Waffles", Price = 2.41 };

                    var daisy = new Employee { FirstName = "戴西Daisy", LastName = "Harrison" };
                    var jack = new Employee { FirstName = "Jack", LastName = "Torrance" };
                    var sue = new Employee { FirstName = "Sue", LastName = "Walkters" };
                    var bill = new Employee { FirstName = "Bill", LastName = "Taft" };
                    var joan = new Employee { FirstName = "约汉Joan", LastName = "Pope" };

                    var productArr = new Product[] { potatoes, fish, milk, bread, cheese };
                    var employeeArr = new Employee[] { daisy, jack, sue, bill, joan };

                    foreach (var item in productArr)
                    {
                        session.SaveOrUpdate(item);
                    }
                    foreach (var item in employeeArr)
                    {
                        session.SaveOrUpdate(item);
                    }
                    transaction.Commit();
                }
            }

            using (var sqlSession = sessionFactory.OpenSession())
            {
                //var sqlQuery = session.CreateSQLQuery("select public.update_employee(2,'1988-01-17 01:49:59.051963'::TIMESTAMP, 'NO:22'::varchar(10));");
                var sqlQuery = sqlSession.CreateSQLQuery("select public.update_employee(:id, :birthday, :sno);");

                var ret = sqlQuery.SetInt64("id", 3)
                    .SetTimestamp("birthday", DateTime.Parse("1988-01-17 01:49:59.051963"))
                    .SetString("sno", "序号3")
                    .UniqueResult<int>();
                    //.ExecuteUpdate(); //  -1
                Console.WriteLine("ret:{0}", ret);
            }
            /*
            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    var stores = session.CreateCriteria(typeof(Store))
                        .List<Store>();

                    foreach (var store in stores)
                    {
                        WriteStorePretty(store);
                    }
                }
            }
            */

            Console.WriteLine("done.");
            Console.ReadKey();
        }

        private static ISessionFactory CreateSessionFactory()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["PostgreConnectionString"].ConnectionString;
            string connectionString = "Server=127.0.0.1;Port=5432;Database=postgres;User Id=postgres;Password=Admin12345;";
            IPersistenceConfigurer config = PostgreSQLConfiguration.PostgreSQL82.ConnectionString(connectionString);

            var cfg = new StoreConfiguration();     // For auto-mapping
            FluentConfiguration configuration = Fluently.Configure()
                .Database(config)
                .Mappings(m => {
                    m.AutoMappings.Add(AutoMap.AssemblyOf<Employee>(cfg)); // your automapping setup here
                    m.FluentMappings.AddFromAssemblyOf<Program>();  // 具有较高的优先级
                })
                .ExposeConfiguration(BuildSchema);
            //configuration.ExposeConfiguration(x => x.SetProperty("hbm2ddl.keywords", "auto-quote"));
            return configuration.BuildSessionFactory();

            //配置方法: https://github.com/jagregory/fluent-nhibernate/wiki/Fluent-configuration
            /*
             var sessionFactory = Fluently.Configure()
              .Database(SQLiteConfiguration.Standard.InMemory)
              .Mappings(m =>
                m.AutoMappings.Add(
                  // your automapping setup here
                  AutoMap.AssemblyOf<YourEntity>(type => type.Namespace.EndsWith("Entities"))))
              .BuildSessionFactory();

            var sessionFactory = Fluently.Configure()
              .Database(SQLiteConfiguration.Standard.InMemory)
              .Mappings(m =>
              {
                m.FluentMappings
                  .AddFromAssemblyOf<YourEntity>();

                m.AutoMappings.Add(
                  // your automapping setup here
                  AutoMap.AssemblyOf<YourEntity>(type => type.Namespace.EndsWith("Entities")));
              })
              .BuildSessionFactory();


            var sessionFactory = Fluently.Configure()
              .Database(SQLiteConfiguration.Standard.InMemory)
              .Mappings(m =>
              {
                m.HbmMappings
                  .AddFromAssemblyOf<YourEntity>();

                m.FluentMappings
                  .AddFromAssemblyOf<YourEntity>();

                m.AutoMappings.Add(
                  // your automapping setup here
                  AutoMap.AssemblyOf<YourEntity>(type => type.Namespace.EndsWith("Entities")));
              })
              .BuildSessionFactory();

             */
        }

        private static void BuildSchema(Configuration config)
        {
            // delete the existing db on each run
            //if (File.Exists(DbFile))
            //    File.Delete(DbFile);

            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            new SchemaExport(config)
                .Create(true, true);
        }
    }
}
