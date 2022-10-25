﻿using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    
    //are going to handle the incoming requests and outgoing responses
    
    public class ItemsController : Controller
    {
        private ItemsServices itemsServices;

        public ItemsController(ItemsServices _itemsServices)
        {
            itemsServices = _itemsServices;
        }



        //a method to open the page, then the user starts typing
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //a method to handle the submission of the form 
        [HttpPost]
        public IActionResult Create(CreateItemViewModel data)
        {   //.....
            try
            {
                itemsServices.AddItem(data);    //to test
                //dynamic object - it builds the declared properties on-the-fly i.e. the moment you declare the property
                //"Message" - it builds in realtime in memory
                ViewBag.Message = "Item successfully inserted in database";
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Item wasn't inserted successfully. Please check your inputs";
            }
            
            return View();
        }

        public IActionResult List()
        {
            var list = itemsServices.GetItems();
            return View(list);
        }

        public IActionResult Details(int id)
        {
            var myItem = itemsServices.GetItem(id);
            return View(myItem);
        }

    }
}
