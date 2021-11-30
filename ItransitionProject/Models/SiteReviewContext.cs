using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ItransitionProject.Models
{
    public partial class SiteReviewContext : DbContext
    {
        public SiteReviewContext()
        {
        }

        public SiteReviewContext(DbContextOptions<SiteReviewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<GroupOverview> GroupOverviews { get; set; } = null!;
        public virtual DbSet<OverTag> OverTags { get; set; } = null!;
        public virtual DbSet<Overview> Overviews { get; set; } = null!;
        public virtual DbSet<RoleUser> RoleUsers { get; set; } = null!;
        public virtual DbSet<TagOverview> TagOverviews { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Data Source=192.168.100.171,1433;Initial Catalog=SiteReview;Integrated Security=False;User ID=Devil;Password=123;", options => options.EnableRetryOnFailure(maxRetryCount: 2,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorNumbersToAdd: new int[] { 2 }));
                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupOverview>(entity =>
            {
                entity.HasKey(e => e.Idgroup)
                    .HasName("PK__GroupOve__CB4260CAF6EA0E0E");

                entity.ToTable("GroupOverview");

                entity.HasIndex(e => e.GroupName, "UQ__GroupOve__6EFCD434DC311C11")
                    .IsUnique();

                entity.Property(e => e.Idgroup).HasColumnName("IDGroup");

                entity.Property(e => e.GroupName).HasMaxLength(50);
            });

            modelBuilder.Entity<OverTag>(entity =>
            {
                entity.HasKey(e => e.IdoverTag)
                    .HasName("PK__OverTag__0DDCDBEE3DFCEC34");

                entity.ToTable("OverTag");

                entity.HasIndex(e => e.Fkoverview, "IX_OverTag_FKOverview");

                entity.HasIndex(e => e.Fktag, "IX_OverTag_FKTag");

                entity.Property(e => e.IdoverTag).HasColumnName("IDOverTag");

                entity.Property(e => e.Fkoverview).HasColumnName("FKOverview");

                entity.Property(e => e.Fktag).HasColumnName("FKTag");

                entity.HasOne(d => d.FkoverviewNavigation)
                    .WithMany(p => p.OverTags)
                    .HasForeignKey(d => d.Fkoverview)
                    .HasConstraintName("FK_Over");

                entity.HasOne(d => d.FktagNavigation)
                    .WithMany(p => p.OverTags)
                    .HasForeignKey(d => d.Fktag)
                    .HasConstraintName("FK_Tag");
            });

            modelBuilder.Entity<Overview>(entity =>
            {
                entity.HasKey(e => e.Idoverview)
                    .HasName("PK__Overview__C7F36726F301D99F");

                entity.ToTable("Overview");

                entity.HasIndex(e => e.Fkgroup, "IX_Overview_FKGroup");

                entity.HasIndex(e => e.Fkuser, "IX_Overview_FKUser");

                entity.Property(e => e.Idoverview).HasColumnName("IDOverview");

                entity.Property(e => e.Fkgroup).HasColumnName("FKGroup");

                entity.Property(e => e.Fkuser).HasColumnName("FKUser");

                entity.Property(e => e.PictureOverview).HasMaxLength(200);

                entity.Property(e => e.ShortDescription).HasMaxLength(1000);

                entity.Property(e => e.TextOverview).HasColumnType("text");

                entity.Property(e => e.Title).HasMaxLength(50);

                entity.HasOne(d => d.FkgroupNavigation)
                    .WithMany(p => p.Overviews)
                    .HasForeignKey(d => d.Fkgroup)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GroupOverview");

                entity.HasOne(d => d.FkuserNavigation)
                    .WithMany(p => p.Overviews)
                    .HasForeignKey(d => d.Fkuser)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Users");
            });

            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.HasKey(e => e.Idrole)
                    .HasName("PK__RoleUser__A1BA16C45A8B17EA");

                entity.HasIndex(e => e.RoleName, "UQ__RoleUser__8A2B6160F9CB9A56")
                    .IsUnique();

                entity.Property(e => e.Idrole).HasColumnName("IDRole");

                entity.Property(e => e.RoleName).HasMaxLength(30);
            });

            modelBuilder.Entity<TagOverview>(entity =>
            {
                entity.HasKey(e => e.Idtag)
                    .HasName("PK__TagOverv__A7023751239D1A2A");

                entity.ToTable("TagOverview");

                entity.HasIndex(e => e.TagName, "UQ__TagOverv__BDE0FD1D8650F9B9")
                    .IsUnique();

                entity.Property(e => e.Idtag).HasColumnName("IDTag");

                entity.Property(e => e.TagName).HasMaxLength(30);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Iduser)
                    .HasName("PK__Users__EAE6D9DF7075EA30");

                entity.HasIndex(e => e.Fkrole, "IX_Users_FKRole");

                entity.Property(e => e.Iduser).HasColumnName("IDUser");

                entity.Property(e => e.Fkrole).HasColumnName("FKRole");

                entity.Property(e => e.UserName).HasMaxLength(50);

                entity.Property(e => e.UserPassword).HasMaxLength(50);

                entity.HasOne(d => d.FkroleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Fkrole)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
