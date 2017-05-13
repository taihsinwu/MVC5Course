using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace MVC5Course.Models
{   
	public  class ProductRepository : EFRepository<Product>, IProductRepository
	{

        public override IQueryable<Product> All()
        {
            return base.All().Where(p => p.IsDeleted == true);
        }

        public IQueryable<Product> All(bool showAll)
        {
            if (showAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
        }

        public Product GetOneDataById(int id)
        {
            return this.All().FirstOrDefault(p => p.ProductId == id);
        }

        public IQueryable<Product> GetAllData(bool active, bool showAll = false)
        {
            IQueryable<Product> all = this.All();
            if (showAll)
            {
                return base.All();
            }
            else
            {
                return this.All();
            }
            //return this.All()
            //           .Where(p => p.Active.HasValue && p.Active.Value == active)
            //           .OrderByDescending(p => p.ProductId).Take(10);
        }
        public void Update(Product product)
        {
            this.UnitOfWork.Context.Entry(product).State = EntityState.Modified;
        }

        public override void Delete(Product entity)
        {
            entity.IsDeleted = true;
        }
    }

	public  interface IProductRepository : IRepository<Product>
	{

	}
}