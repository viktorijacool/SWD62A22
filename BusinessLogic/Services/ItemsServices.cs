using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BusinessLogic.Services
{
    //Item.cs (Domain) is used to model/shape/generate/engineer the database
    //e.g. User.cs

    //CreateItemViewModel.cs (BusinessLogic) is used to display the data of the database


    public class ItemsServices
    {

        //Constructor Injection
        //Dependency Injection is a design pattern which handles the creation of instances in a centralised location for better efficiency

        private ItemsRepository ir;
        
        public ItemsServices(ItemsRepository _itemRepository) 
        {
            ir = _itemRepository;
        }

        public void AddItem(CreateItemViewModel item)
        {
            
            if (ir.GetItems().Any(i => i.Name == item.Name))
                throw new Exception("Item with the same name already exists");
            else
            {
                ir.AddItem(new Domain.Models.Item()
                {
                    CategoryId = item.CategoryId,   //AutoMapper
                    Description = item.Description,
                    Name = item.Name,
                    PhotoPath = item.PhotoPath,
                    Price = item.Price,
                    Stock = item.Stock
                });
            }
        
        }

        public void DeleteItem(int id)
        {

        }

        public void Checkout(int id)
        {

        }

        //it is not recommended that you use the Domain Models as a return type
        //in other words, do not use the classes that model the database to transfer data into the presentation layer

        //Item contains all necessary data from the database
        //ItemViewModel is used to display a certain info from the database
        public IQueryable<ItemViewModel> GetItems()
        {
            
            //Linq
            //SQL may be complicated vis-a-vis Linq e.g. you need liner joins
            //Linq is more C#-like rather than having to learn a completely new language
            //Linq is compiled therefore the compile will point out my errors for you

            //note: I am wrapping item info into List<ItemViewModel> because the ir.GetItems() returns List<Item>
            
            
            var list = from i in ir.GetItems()          //flatten this into 1 line using AutoMapper
                       select new ItemViewModel()
                       {
                           Id = i.Id,
                           Category = i.Category.Title,
                           Description = i.Description,
                           Name = i.Name,
                           PhotoPath = i.PhotoPath,
                           Price = i.Price,
                           Stock = i.Stock

                       };
            return list;
        }


        public ItemViewModel GetItem(int id)
        {
            return GetItems().SingleOrDefault(x => x.Id == id);     //SingleOrDefault - returns one record or none
        }



        public IQueryable<ItemViewModel> Search(string keyword)
        {
            return GetItems().Where(x => x.Name.Contains(keyword));     //Like %%
        }


        //note: List vs IQuerable

        //List is less efficient
        //1st call result would have been that 1000 items fetched and loaded into the server's memory
        //2nd call result would have implemented a filter on those 1000 items and result would have been also loaded into memory
        //3rd call result would have implemented a filter on those 500 items and result would have been also loaded into memory
        //Result: too much memory is used

        //IQuerable is more efficient
        //1st call result would have been a preparation of a Linq query and query would have been placed in memory
        //2nd call result would have been an amendment to the 1st Linq query to include the Where clause
        //3rd call result would have been a further amendment to the same Linq query to include an additional Where clause

        //When does the execution take place? 
        //Answer: the execution happens once only when you convert IQuerable to List (or that happens) the moment you pass data to the View
        
        //IQuerable makes the prepared Linq statement run and therefore filters the data within the database

        public IQueryable<ItemViewModel> Search(string keyword, double minPrice, double maxPrice)
        {
            return Search(keyword).Where(x => x.Price >= minPrice && x.Price <= maxPrice);
        }


    }

    
}
