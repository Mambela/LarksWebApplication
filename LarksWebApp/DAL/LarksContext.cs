using LarksWebApp.Model.BaseModels;
using LarksWebApp.Models;
using LarksWebApp.Models.CoordinatesModel;
using LarksWebApp.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LarksWebApp.DAL
{
    public class LarksContext : IdentityDbContext<ApplicationUser>
    {

        public LarksContext(DbContextOptions<LarksContext> options) : base(options)
        {
        }

        public DbSet<CountryModel> Countries { get; set; }

        public DbSet<ProvinceModel> Provinces { get; set; }

        public DbSet<MunicipalityModel> Municipalities { get; set; }

        public DbSet<DistrictModel> Districts { get; set; }

        public DbSet<SuburbModel> Suburb { get; set; }


        public DbSet<StreetModel> Streets { get; set; }
        public DbSet<SuburbStreets> SuburbStreets { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<AddressModel> Addresses { get; set; }
        public DbSet<ProvinceCoordinates> ProvinceCoordinates { get; set; }
        public DbSet<MunicipalityCoordinates> MunicipalityCoordinates { get; set; }
        public DbSet<SuburbCoordinates> SuburbCoordinates { get; set; }
        public DbSet<StreetCoordinates> StreetCoordinates { get; set; }
        public DbSet<CityCoordinates> CitiesCoordinates { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubcategoryModel> Subcategories { get; set; }
        public DbSet<BuildingTypeModel> BuildingType { get; set; }

        public DbSet<LocationModel> Location { get; set; }

        public DbSet<LocationImageModel> LocationImage { get; set; }

        public DbSet<HooksModel> HooksModel { get; set; }

        public DbSet<AuthToken> Tokens { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

       public DbSet<LocationModelSubcategoryModel> LocationModelSubcategoryModel { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                //optionsBuilder.UseSqlServer("Data Source=SQL8001.site4now.net;Initial Catalog=db_a861d4_larksuat;User Id=db_a861d4_larksuat_admin;Password=k2SRQ3RyRXV.R5j");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<SuburbModel>()
                .HasOne(s => s.Municipality)
                .WithMany(m => m.Suburbs)
                .HasForeignKey(s => s.MunicipalityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SuburbStreets>().HasKey(n => new { n.StreetsID, n.SuburbId });
            modelBuilder.Entity<LocationModel>().Property(p => p.Lat).HasPrecision(8, 6);
            modelBuilder.Entity<LocationModel>().Property(p => p.Lng).HasPrecision(9, 6);

            // Soft-deleted rows are excluded from every query against these entities by default.
            modelBuilder.Entity<StreetModel>().HasQueryFilter(s => !s.IsDeleted);
            modelBuilder.Entity<MunicipalityModel>().HasQueryFilter(m => !m.IsDeleted);
            modelBuilder.Entity<SuburbModel>().HasQueryFilter(s => !s.IsDeleted);

            modelBuilder.Entity<LocationModelSubcategoryModel>().HasKey(n => new { n.LocationId, n.SubcategoriesId });
            

            #region SQL SERVER 
            //modelBuilder.HasDefaultSchema("LARKS_UAT");
            //modelBuilder.Entity<IdentityUser>(entity =>
            //{
            //    entity.ToTable(name: "Users");
            //});
            //modelBuilder.Entity<IdentityRole>(entity =>
            //{
            //    entity.ToTable(name: "Role");
            //});
            //modelBuilder.Entity<IdentityUserRole<string>>(entity =>
            //{
            //    entity.ToTable("UserRoles");
            //});
            //modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            //{
            //    entity.ToTable("UserClaims");
            //});
            //modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            //{
            //    entity.ToTable("UserLogins");
            //});
            //modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            //{
            //    entity.ToTable("RoleClaims");
            //});
            //modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            //{
            //    entity.ToTable("UserTokens");
            //});
            #endregion
            base.OnModelCreating(modelBuilder);
        }

        #region Additional Database Mods

        // Entities added for created date and Update date. 

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            onBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void onBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                //for entities that inherit from BaseEntity
                //set UpdateDate/ CreatedDate appropriately
                //

                if (entry.Entity is TimeStampsEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            //set the updated date to now
                            trackable.UpdatedDate = utcNow;

                            //mark property as dont update
                            entry.Property("CreatedDate").IsModified = false;
                            break;

                        case EntityState.Added:

                            //set both updated and created date to now
                            trackable.CreatedDate = utcNow;
                            trackable.UpdatedDate = utcNow;

                            break;
                    }
                }


            }

        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default)
        {

            onBeforeSaving();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess,
                 cancellationToken);
        }

        #endregion



    }
}
