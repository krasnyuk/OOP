using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberRMS.Models
{
    public class IngredientsViewModel
    {
        public int IngredientID { get; set; }
        public string Title { get; set; }
        public string LongDescription { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string CategoryName { get; set; }
        public List<Tmodel> SubIngredient { get; set; }

    }
    public class Tmodel
    {
        public int id { get; set; }
    }
}