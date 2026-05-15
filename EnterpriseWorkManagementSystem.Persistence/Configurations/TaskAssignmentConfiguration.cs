using EnterpriseWorkManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseWorkManagementSystem.Persistence.Configurations
{
    public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
    {
        public void Configure(EntityTypeBuilder<TaskAssignment> builder)
        {
            builder.ToTable("TaskAssignments");

            builder.HasKey(x => new { x.TaskItemId, x.UserId });

            builder.HasOne(x => x.TaskItem)
                .WithMany(x => x.Assignments)
                .HasForeignKey(x => x.TaskItemId);

            builder.HasOne(x => x.User)
                .WithMany(x => x.TaskAssignments)
                .HasForeignKey(x => x.UserId);
        }
    }
}
