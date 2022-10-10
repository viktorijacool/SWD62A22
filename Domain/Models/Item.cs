using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    //Domain: will be a project referenced by all the other layers and will
    //        hold the data containers i.e. Models
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string PhotoPath { get; set; }
    }
}
