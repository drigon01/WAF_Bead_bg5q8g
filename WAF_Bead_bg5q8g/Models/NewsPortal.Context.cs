﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Service.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class News_PortalEntities : DbContext
    {
        public News_PortalEntities()
            : base("name=News_PortalEntities")
        {
            Articles = Set<Article>();
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        internal virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }

    public override int SaveChanges()
    {
      return base.SaveChanges();
    }
  }
}
