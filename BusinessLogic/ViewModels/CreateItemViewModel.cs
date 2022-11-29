using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BusinessLogic.ViewModels
{
    //is a selection of the required data (a filter)

    public class CreateItemViewModel
    {
        public List<CategoryViewModel> Categories { get; set; }

        //Validator (for user-friendly coding approach)
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name cannot be blank")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        //Validator
        [Range(1, 10000, ErrorMessage = "Range is between 1 - 10 000 EUR")]
        public double Price { get; set; }

        public string Description { get; set; }

        //Property for renaming for the end user
        [Display(Name = "Photo")]
        public string PhotoPath { get; set; }

        public int Stock { get; set; }

    }
}
