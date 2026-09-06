using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using School.Domain.Progress;

namespace School.Infrastructure.Persistence.Configurations;

public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> builder)
    {
        builder.ToTable("lesson_progress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.WatchedSeconds)
            .IsRequired();

        builder.Property(x => x.Completed)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();

        // Foreign key: student_id -> users
        builder.HasOne(x => x.Student)
            .WithMany(u => u.LessonProgresses)
            .HasForeignKey(x => x.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Foreign key: lesson_id -> lessons
        builder.HasOne(x => x.Lesson)
            .WithMany(l => l.Progresses)
            .HasForeignKey(x => x.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique: one progress per student per lesson
        builder.HasIndex(x => new { x.StudentId, x.LessonId })
            .IsUnique();

        builder.HasIndex(x => x.StudentId);
    }
}