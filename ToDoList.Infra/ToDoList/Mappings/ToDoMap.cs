using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoList.Domain.ToDoList.Entities;

namespace ToDoList.Infra.ToDoList.Mappings;
public class ToDoMap : IEntityTypeConfiguration<ToDo> {
    public void Configure(EntityTypeBuilder<ToDo> builder) {
        builder.ToTable("ToDo");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Description)
            .IsRequired(true)
            .HasColumnName("Description")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(150);

        builder.Property(x => x.Deleted)
            .IsRequired(true)
            .HasColumnName("Deleted")
            .HasColumnType("BIT")
            .HasDefaultValue(false);

        builder.Property(x => x.Done)
            .IsRequired(true)
            .HasColumnName("Done")
            .HasColumnType("BIT")
            .HasDefaultValue(false);

        builder.Property(x => x.CreatedAt)
            .IsRequired(true)
            .HasColumnName("CreatedAt")
            .HasColumnType("SMALLDATETIME");
    }
}
