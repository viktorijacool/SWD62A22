using BusinessLogic.ViewModels;
using DataAccess.Repositories;
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

    }

    
}
