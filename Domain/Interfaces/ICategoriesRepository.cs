using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Interfaces
{
    //Intefaces are created to be inherited (not a class, but a template)
    public interface ICategoriesRepository
    {
        IQueryable<Category> GetCategories();

    }
}
