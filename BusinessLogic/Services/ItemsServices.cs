using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

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
        //in other words, do not use the classes tha model the database to transfer data into the presentation layer
        //public IQueryable<ItemViewModel> GetItems()
        //{
        //    var list = from i in ir.GetItems()
        //               select new CreateItemViewModel()
        //               {
        //                   Id = i.Id;
        //                    Category = i.Category.Title;
                                //...

        //               }
            //return list;
        //}

    }

    
}
