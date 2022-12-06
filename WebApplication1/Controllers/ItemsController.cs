﻿using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApplication1.ActionFilters;

namespace WebApplication1.Controllers
{

    //are going to handle the incoming requests and outgoing responses

    public class ItemsController : Controller
    {
        private ItemsServices itemsServices;
        private IWebHostEnvironment host;
        private CategoriesServices categoriesServices;

        public ItemsController(ItemsServices _itemsServices, IWebHostEnvironment _host, CategoriesServices _categoriesServices)
        {
            itemsServices = _itemsServices;
            host = _host;
            categoriesServices = _categoriesServices;
        }



        //a method to open the page, then the user starts typing
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            var categories = categoriesServices.GetCategories();
            CreateItemViewModel myModel = new CreateItemViewModel();
            myModel.Categories = categories.ToList();

            return View(myModel);
        }

        //a method to handle the submission of the form 
        [HttpPost]
        [Authorize]     //only checks whether a user is logged in or not
        public IActionResult Create(CreateItemViewModel data, IFormFile file)
        {   //.....
            try
            {
                if (ModelState.IsValid)     //a built-in manager
                {
                    //Adding Validation

                    //check if the category exists in the db
                    //if not
                    //ModelState.AddModelError("CategoryId", "Category is not valid");
                    //return View(data);

                    string username = User.Identity.Name;   //gives you the email/username of the currently logged in user

                    if (file != null)
                    {
                        //1. change filename
                        string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                        //2. the absolute path of the folder where the image is going...
                        //e.g. D:\MCAST\Enterprise Proramming\EnterpriseProgrammingSolution\WebApplication1\wwwroot\Images\

                        string absolutePath = host.WebRootPath;

                        //3. saving file
                        using (System.IO.FileStream fsOut = new System.IO.FileStream(absolutePath + "\\Images\\" + uniqueFilename, System.IO.FileMode.CreateNew))
                        {
                            file.CopyTo(fsOut);
                        }

                        //4. save the path to the image in the database
                        //http://localhost:xxxx/Images/filename.jpg
                        data.PhotoPath = "/Images/" + uniqueFilename;
                    }



                    //data.Author = username;

                    itemsServices.AddItem(data);    //to test
                                                    //dynamic object - it builds the declared properties on-the-fly i.e. the moment you declare the property
                                                    //"Message" - it builds in realtime in memory
                    ViewBag.Message = "Item successfully inserted in database";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Item wasn't inserted successfully. Please check your inputs";
            }

            var categories = categoriesServices.GetCategories();
            data.Categories = categories.ToList();

            return View(data);
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

        [HttpPost]
        public IActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return RedirectToAction("List");
            }
            else
            {
                var list = itemsServices.Search(keyword);
                return View("List", list);
            }
            
        }

        [HttpGet]
        [Authorize]
        [FilePermissionAuthorization]
        public IActionResult Delete(int id)
        {
            try
            {
                itemsServices.DeleteItem(id);

                //ViewBag will not work here because Viewbag is lost when there is a redirection
                //TempData survives redirections (up to 1 redirection)
                TempData["message"] = "Item has been deleted";
            }
            catch(Exception ex)
            {
                TempData["error"] = "Item has not been deleted";
            }
            return RedirectToAction("List");
        }

        [HttpGet]   //this method will be executed first when a user loads the page so they can see the current info
        public IActionResult Edit(int id) 
        {
            var originalItem = itemsServices.GetItem(id);
            var categories = categoriesServices.GetCategories();

            CreateItemViewModel model = new CreateItemViewModel();
            model.Categories = categories.ToList();
            model.Name = originalItem.Name;
            model.CategoryId = categories.SingleOrDefault(x=>x.Title == originalItem.Category).Id;
            model.Description = originalItem.Description;
            model.Price = originalItem.Price;
            model.PhotoPath = originalItem.PhotoPath;
            model.Stock = originalItem.Stock;

            return View(model);
        }

        public IActionResult Edit(int id, CreateItemViewModel data, IFormFile file)
        {
            try
            {
                var oldItem = itemsServices.GetItem(id);

                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        //1. change filename
                        string uniqueFilename = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(file.FileName);

                        //2. the absolute path of the folder where the image is going...
                        //e.g. D:\MCAST\Enterprise Proramming\EnterpriseProgrammingSolution\WebApplication1\wwwroot\Images\

                        string absolutePath = host.WebRootPath;

                        //3. saving file
                        using (System.IO.FileStream fsOut = new System.IO.FileStream(absolutePath + "\\Images\\" + uniqueFilename, System.IO.FileMode.CreateNew))
                        {
                            file.CopyTo(fsOut);
                        }

                        //4. save the path to the image in the database
                        //http://localhost:xxxx/Images/filename.jpg
                        data.PhotoPath = "/Images/" + uniqueFilename;

                        //5. delete the old physical file (image)
                        string absolutePathOfOldImage = host.WebRootPath + "\\Images\\" + Path.GetFileName(oldItem.PhotoPath);
                        
                        if(System.IO.File.Exists(absolutePathOfOldImage) == true)
                        {
                            System.IO.File.Delete(absolutePathOfOldImage);
                        }
                    }
                    else
                    {
                        data.PhotoPath = oldItem.PhotoPath;
                    }

                    itemsServices.EditItem(id, data);    

                    ViewBag.Message = "Item has been updated successfully";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Item hasn't been updated successfully. Please check your inputs";
            }

            var categories = categoriesServices.GetCategories();
            data.Categories = categories.ToList();

            return View(data);
        }
    }

}

