using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Cars.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Cars> Cars { get; set; }

        public async Task<List<Cars>> GetCars(string year, string make, string model, string trim, string filter, bool? paging, int? page, int? perPage)
        {
            var yearParam = new SqlParameter("@year", year);
            var makeParam = new SqlParameter("@make", make);
            var modelParam = new SqlParameter("@model", model);
            var trimParam = new SqlParameter("@trim", trim);
            var filterParam = new SqlParameter(@"filter", filter);

            var query = "GetCars @year, @make, @model, @trim, @filter";

            if (paging != null && paging.Value == true)
            {
                var pagingParam = new SqlParameter("@paging", paging);
                var pageParam = new SqlParameter("@page", page);
                var perPageParam = new SqlParameter("@perPage", perPage);
                query += " , @paging, @page, @perPage";
            }
            else
            {
                var result = await Database.SqlQuery<Cars>(query, yearParam, makeParam, modelParam, trimParam, filterParam).ToListAsync();
                return result;
            }
            return new List<Cars>();
        }

        public async Task<List<string>> GetYears()
        {
            return await this.Database
                .SqlQuery<string>("GetYears").ToListAsync();
        }

        public async Task<List<string>> GetMakes(string year)
        {
            var yearParam = new SqlParameter("@year", year);
            return await this.Database
                .SqlQuery<string>("GetMakes @year", yearParam).ToListAsync();
        }

        public async Task<List<string>> GetModels(string make)
        {
            var makeParam = new SqlParameter("@make", make);
            return await this.Database
                .SqlQuery<string>("GetModels @year, @make", makeParam).ToListAsync();
        }

        public async Task<List<string>> GetTrim(string model)
        {
            var modelParam = new SqlParameter("@model", model);
            return await this.Database
                .SqlQuery<string>("GetTrim @year, @make, @model", modelParam).ToListAsync();
        }

        public async Task<List<string>> GetFilters(string trim)
        {
            var trimParam = new SqlParameter("@trim", trim);
            return await this.Database
                .SqlQuery<string>("GetFilters @year, @make, @model, @trim", trimParam).ToListAsync();
        }
    }
}