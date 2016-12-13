﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestNHibernatePostgreSQL.Entities
{
    public class Product
    {
        public virtual int Id { get; protected set; }
        public virtual string Name { get; set; }
        public virtual double Price { get; set; }
    }
}
