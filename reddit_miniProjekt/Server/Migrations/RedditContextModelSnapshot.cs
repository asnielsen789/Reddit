﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using reddit_miniProjekt.Server.Context;

#nullable disable

namespace reddit_miniProjekt.Server.Migrations
{
    [DbContext(typeof(RedditContext))]
    partial class RedditContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.Comment", b =>
                {
                    b.Property<long>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("RedditThreadId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CommentId");

                    b.HasIndex("RedditThreadId");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.RedditThread", b =>
                {
                    b.Property<long>("RedditThreadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("RedditThreadId");

                    b.ToTable("Threads", (string)null);
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.User", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.Vote", b =>
                {
                    b.Property<long>("VoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("CommentId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Evaluation")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("RedditThreadId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VoteId");

                    b.HasIndex("CommentId");

                    b.HasIndex("RedditThreadId");

                    b.HasIndex("UserId");

                    b.ToTable("Vote");
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.Comment", b =>
                {
                    b.HasOne("reddit_miniProjekt.Shared.Models.RedditThread", null)
                        .WithMany("Comments")
                        .HasForeignKey("RedditThreadId");
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.Vote", b =>
                {
                    b.HasOne("reddit_miniProjekt.Shared.Models.Comment", null)
                        .WithMany("Votes")
                        .HasForeignKey("CommentId");

                    b.HasOne("reddit_miniProjekt.Shared.Models.RedditThread", null)
                        .WithMany("Votes")
                        .HasForeignKey("RedditThreadId");

                    b.HasOne("reddit_miniProjekt.Shared.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.Comment", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("reddit_miniProjekt.Shared.Models.RedditThread", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Votes");
                });
#pragma warning restore 612, 618
        }
    }
}