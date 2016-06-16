using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace Service.Models
{
  public class AdministratorContext : News_PortalEntities
  {
    public override int SaveChanges()
    {
      try
      {
        return base.SaveChanges();
      }
      catch (DbEntityValidationException e)
      {
        //var newException = new FormattedDbEntityValidationException(e);
        throw new Exception();
      }
    }
  }
}