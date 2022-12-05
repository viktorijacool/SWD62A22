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

        public Item GetItem(int id)
        {
            return context.Items.SingleOrDefault(x => x.Id == id);
        }

        public void EditItem(Item updatedItem)
        {
            //1. get the original item from the database 

            var originalItem = GetItem(updatedItem.Id);     //the Id should never be allowed to be changed


            //2. update the details which were supposed to be updated one by one

            originalItem.Name = updatedItem.Name;
            originalItem.PhotoPath = updatedItem.PhotoPath;
            originalItem.Price = updatedItem.Price;
            originalItem.Stock = updatedItem.Stock;
            originalItem.Description = updatedItem.Description;
            originalItem.CategoryId = updatedItem.CategoryId;       //we change the foreign key, NOT the navigational property

            context.SaveChanges();
        }
    }
}
