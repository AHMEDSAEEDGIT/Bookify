using Bookify.DataAccess.Repository.IRepository;
using Bookify.DataAcess.Data;
using Bookify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.DataAccess.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            this._db = db;
        }

        public void Update (Product product)
        {
            _db.Products.Update(product);
        }

    }
}
