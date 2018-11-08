﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InkaPharmacy.Api.Employee.Domain.Entity
{
    using InkaPharmacy.Api.Common.Application;
  
    public class Employee
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Last_name1 { get; set; }
        public virtual string Last_name2 { get; set; }
        public virtual string Address { get; set; }
        public virtual string Telephone { get; set; }
        public virtual long Role_id { get; set; }
        public virtual long Store_id { get; set; }
        public virtual string Username { get; set; }
        public virtual string Password { get; set; }
        public virtual string Email { get; set; }
        public virtual int Status { get; set; }
       
        public Employee()
        {
        }

        public virtual Notification validateForLogin()
        {
            Notification notification = new Notification();

            if (this == null)
            {
                notification.addError("Employee doesn't logueado");
            }

            return notification;
        }

    }
}
