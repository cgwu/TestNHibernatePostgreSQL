using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestNHibernatePostgreSQL.Entities
{
    public class Employee
    {
        public virtual long Id { get; protected set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual DateTime Birthday { get; set; }
        public virtual string Study_No { get; set; }
        //public virtual Store Store { get; set; }
    }
}
