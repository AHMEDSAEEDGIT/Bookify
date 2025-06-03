using Bookify.DataAccess.Repository.IRepository;
using Bookify.DataAcess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public ICategoryRepository CategoryRepository { get; private set; }

        public IProductRepository ProductRepository {get; private set; }

        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            CategoryRepository = new CategoryRepository(dbContext);
            ProductRepository = new ProductRepository(dbContext);
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
