using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        public DbSet<CommentOverview> CommentOverviews { get; set; }
        public DbSet<GroupOverview> GroupOverviews { get; set; }
        public  DbSet<OverCom> OverComs { get; set; }
        public  DbSet<OverTag> OverTags { get; set; } 
        public  DbSet<Overview> Overviews { get; set; }
        public  DbSet<RoleUser> RoleUsers { get; set; }
        public DbSet<TagOverview> TagOverviews { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=USER-PC\\SQL_EXPRESS;Database=SiteReview;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentOverview>(entity =>
            {
                entity.HasKey(e => e.IdcommentOverview)
                    .HasName("PK__CommentO__0C524CEFCCA9ABA9");

                entity.ToTable("CommentOverview");

                entity.Property(e => e.IdcommentOverview).HasColumnName("IDCommentOverview");

                entity.Property(e => e.Comment).HasMaxLength(100);

                entity.Property(e => e.DateComment).HasColumnType("datetime");
            });

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

            modelBuilder.Entity<OverCom>(entity =>
            {
                entity.HasKey(e => e.IdoverCom)
                    .HasName("PK__OverCom__0DDCDBEEDCBEF12B");

                entity.ToTable("OverCom");

                entity.Property(e => e.IdoverCom).HasColumnName("IDOverCom");

                entity.Property(e => e.Fkcomment).HasColumnName("FKComment");

                entity.Property(e => e.Fkoverviews).HasColumnName("FKOverviews");

                entity.Property(e => e.Fkusers).HasColumnName("FKUsers");

                entity.HasOne(d => d.FkcommentNavigation)
                    .WithMany(p => p.OverComs)
                    .HasForeignKey(d => d.Fkcomment)
                    .HasConstraintName("FK_Com");

                entity.HasOne(d => d.FkoverviewsNavigation)
                    .WithMany(p => p.OverComs)
                    .HasForeignKey(d => d.Fkoverviews)
                    .HasConstraintName("FK_OverCom");

                entity.HasOne(d => d.FkusersNavigation)
                    .WithMany(p => p.OverComs)
                    .HasForeignKey(d => d.Fkusers)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCom");
            });

            modelBuilder.Entity<OverTag>(entity =>
            {
                entity.HasKey(e => e.IdoverTag)
                    .HasName("PK__OverTag__0DDCDBEE72D02351");

                entity.ToTable("OverTag");

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
                    .HasName("PK__Overview__C7F36726A94B49C2");

                entity.ToTable("Overview");

                entity.Property(e => e.Idoverview).HasColumnName("IDOverview");

                entity.Property(e => e.DateOverview).HasColumnType("datetime");

                entity.Property(e => e.Fkgroup).HasColumnName("FKGroup");

                entity.Property(e => e.Fkuser).HasColumnName("FKUser");

                entity.Property(e => e.ImageUrl).HasMaxLength(400);

                entity.Property(e => e.StorageName).HasMaxLength(400);

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
