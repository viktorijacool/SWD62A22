using BusinessLogic.Services;
using BusinessLogic.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Create()
        {
            var categories = categoriesServices.GetCategories();
            CreateItemViewModel myModel = new CreateItemViewModel();
            myModel.Categories = categories.ToList();

            return View(myModel);
        }

        //a method to handle the submission of the form 
        [HttpPost]
        public IActionResult Create(CreateItemViewModel data, IFormFile file)
        {   //.....
            try
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
                }





                itemsServices.AddItem(data);    //to test
                //dynamic object - it builds the declared properties on-the-fly i.e. the moment you declare the property
                //"Message" - it builds in realtime in memory
                ViewBag.Message = "Item successfully inserted in database";
            }
            catch(Exception ex)
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

    }
}
