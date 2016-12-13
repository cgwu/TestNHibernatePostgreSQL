using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestNHibernatePostgreSQL.Entities;

namespace TestNHibernatePostgreSQL.Mappings
{
    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName);
            Map(x => x.LastName);
            Map(x => x.Birthday);
            Map(x => x.Study_No).Column("STUDY_NUMBER");
            //References(x => x.Store);
        }
    }
}
