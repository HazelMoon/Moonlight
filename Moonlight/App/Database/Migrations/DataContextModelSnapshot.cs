﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moonlight.App.Database;

#nullable disable

namespace Moonlight.App.Database.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.PostLike", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PostId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostLikes");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.WordFilter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Filter")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("WordFilters");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.Server", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Cpu")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Disk")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DockerImageIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ImageId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MainAllocationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Memory")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NodeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OverrideStartupCommand")
                        .HasColumnType("TEXT");

                    b.Property<int>("ServiceId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ImageId");

                    b.HasIndex("MainAllocationId");

                    b.HasIndex("NodeId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerAllocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Port")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServerId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServerNodeId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.HasIndex("ServerNodeId");

                    b.ToTable("ServerAllocations");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerDockerImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ServerImageId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServerImageId");

                    b.ToTable("ServerDockerImages");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AllocationsNeeded")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DonateUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("InstallDockerImage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InstallScript")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("InstallShell")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OnlineDetection")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ParseConfigurations")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StartupCommand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StopCommand")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UpdateUrl")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("ServerImages");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerImageVariable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ServerImageId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ServerImageId");

                    b.ToTable("ServerImageVariables");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerNode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fqdn")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("HttpPort")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("UseSsl")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ServerNodes");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerVariable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ServerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ServerId");

                    b.ToTable("ServerVariables");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Coupon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Percent")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.CouponUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CouponId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CouponId");

                    b.HasIndex("UserId");

                    b.ToTable("CouponUses");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.GiftCode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("GiftCodes");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.GiftCodeUse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GiftCodeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GiftCodeId");

                    b.HasIndex("UserId");

                    b.ToTable("GiftCodeUses");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigJson")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Duration")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxPerUser")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Stock")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConfigJsonOverride")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Nickname")
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("RenewAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Suspended")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.ServiceShare", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.HasIndex("UserId");

                    b.ToTable("ServiceShares");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Price")
                        .HasColumnType("REAL");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Tickets.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("CreatorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Open")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Priority")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Tries")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Tickets.TicketMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Attachment")
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsSupport")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SenderId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("TicketId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("TicketId");

                    b.ToTable("TicketMessages");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Avatar")
                        .HasColumnType("TEXT");

                    b.Property<double>("Balance")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Flags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Permissions")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TokenValidTimestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("TotpKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.Post", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.PostComment", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Community.Post", null)
                        .WithMany("Comments")
                        .HasForeignKey("PostId");

                    b.Navigation("Author");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.PostLike", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Community.Post", null)
                        .WithMany("Likes")
                        .HasForeignKey("PostId");

                    b.HasOne("Moonlight.App.Database.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.Server", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerImage", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerAllocation", "MainAllocation")
                        .WithMany()
                        .HasForeignKey("MainAllocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerNode", "Node")
                        .WithMany()
                        .HasForeignKey("NodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Store.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("MainAllocation");

                    b.Navigation("Node");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerAllocation", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Servers.Server", null)
                        .WithMany("Allocations")
                        .HasForeignKey("ServerId");

                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerNode", null)
                        .WithMany("Allocations")
                        .HasForeignKey("ServerNodeId");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerDockerImage", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerImage", null)
                        .WithMany("DockerImages")
                        .HasForeignKey("ServerImageId");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerImageVariable", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Servers.ServerImage", null)
                        .WithMany("Variables")
                        .HasForeignKey("ServerImageId");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerVariable", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Servers.Server", null)
                        .WithMany("Variables")
                        .HasForeignKey("ServerId");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.CouponUse", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Store.Coupon", "Coupon")
                        .WithMany()
                        .HasForeignKey("CouponId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.User", null)
                        .WithMany("CouponUses")
                        .HasForeignKey("UserId");

                    b.Navigation("Coupon");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.GiftCodeUse", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Store.GiftCode", "GiftCode")
                        .WithMany()
                        .HasForeignKey("GiftCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.User", null)
                        .WithMany("GiftCodeUses")
                        .HasForeignKey("UserId");

                    b.Navigation("GiftCode");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Product", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Store.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Service", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Store.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.ServiceShare", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.Store.Service", null)
                        .WithMany("Shares")
                        .HasForeignKey("ServiceId");

                    b.HasOne("Moonlight.App.Database.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Transaction", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", null)
                        .WithMany("Transactions")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Tickets.Ticket", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Moonlight.App.Database.Entities.Store.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId");

                    b.Navigation("Creator");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Tickets.TicketMessage", b =>
                {
                    b.HasOne("Moonlight.App.Database.Entities.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.HasOne("Moonlight.App.Database.Entities.Tickets.Ticket", null)
                        .WithMany("Messages")
                        .HasForeignKey("TicketId");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Community.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.Server", b =>
                {
                    b.Navigation("Allocations");

                    b.Navigation("Variables");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerImage", b =>
                {
                    b.Navigation("DockerImages");

                    b.Navigation("Variables");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Servers.ServerNode", b =>
                {
                    b.Navigation("Allocations");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Store.Service", b =>
                {
                    b.Navigation("Shares");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.Tickets.Ticket", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("Moonlight.App.Database.Entities.User", b =>
                {
                    b.Navigation("CouponUses");

                    b.Navigation("GiftCodeUses");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
