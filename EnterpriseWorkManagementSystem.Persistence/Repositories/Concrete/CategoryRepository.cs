using EnterpriseWorkManagementSystem.Application.Abstractions.Persistence;
using EnterpriseWorkManagementSystem.Domain.Entities;
using EnterpriseWorkManagementSystem.Persistence.Context;
using EnterpriseWorkManagementSystem.Persistence.Repositories.Generic;


namespace EnterpriseWorkManagementSystem.Persistence.Repositories.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
