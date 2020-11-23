﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SDIIS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SDIIS_DatabaseEntities : DbContext
    {
        public SDIIS_DatabaseEntities()
            : base("name=SDIIS_DatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Menu_Item> Menu_Items { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<Module_Action> Module_Actions { get; set; }
        public virtual DbSet<Module_Controller> Module_Controllers { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Job_Position> Job_Positions { get; set; }
        public virtual DbSet<Paypoint> Paypoints { get; set; }
        public virtual DbSet<Race> Races { get; set; }
        public virtual DbSet<Salary_Level> Salary_Levels { get; set; }
        public virtual DbSet<Service_Office> Service_Offices { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<District> Districts { get; set; }
        public virtual DbSet<Province> Provinces { get; set; }
    }
}
