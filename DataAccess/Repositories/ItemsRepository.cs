using DataAccess.Context;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccess.Repositories
{
    //Dependency Injection: it centralises the creation of instances to manage efficiently these inside memory
    
    public class ItemsRepository
    {
        private ShoppingCartContext context { get; set; }


        //Constructor Injection - we are shifting creation of instances such as ShopingCartContext to a more centralised place i.e. Startup.cs

        //declaring by using constructor injection that the ItemRepository when consumed it must be given an instance of ShoppingCartContext

        public ItemsRepository(ShoppingCartContext _context)
        {
            context = _context;
        }

        //in the data access we code methods that directly add/read data to/from the database

        public IQueryable<Item> GetItems()
        { 
            return context.Items; 
        }

        public void AddItem(Item i) 
        { 
            context.Items.Add(i);
            context.SaveChanges();
        }

        public void DeleteItem(Item i)
        {
            context.Items.Remove(i);
            context.SaveChanges();
        }
    }
}
