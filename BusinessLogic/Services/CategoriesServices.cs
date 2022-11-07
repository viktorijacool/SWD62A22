using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLogic.Services
{
    public class CategoriesServices
    {
        private ICategoriesRepository cr;

        public CategoriesServices(ICategoriesRepository _categoriesRepository)
        {
            cr = _categoriesRepository;
        }

        //The method below gets all the categories from the database
        public IQueryable<CategoryViewModel> GetCategories()
        {
            //AutoMapper - still to be implemented to replace the following
            var list = from c in cr.GetCategories() 
                       select new CategoryViewModel()
                       {
                           Id = c.Id,
                           Title = c.Title

                       };
            return list;
        }

    }
}
