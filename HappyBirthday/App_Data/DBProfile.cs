using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HappyBirthday.Models
{
    public class UserProfile
    {

        [Key]
        public int Id { get; set; }

       
       
        public string Title { get; set; }

      
        public string FirstName { get; set; }

   
        public string LastName { get; set; }

        
        public int BirthDay { get; set; }

        public int BirthMonth { get; set; }

        public string TelephoneNumber {get; set;}
      
        public string Filename { get; set; }
    }
  

    public partial class BirthDayContext : DbContext
    {
        public BirthDayContext()
            : base("name=BirthDayContext")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }




}