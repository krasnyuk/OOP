//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MemberRMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ingredient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ingredient()
        {
            this.CartDetails = new HashSet<CartDetails>();
            this.ProductIngredient = new HashSet<ProductIngredient>();
            this.ProductIngredient1 = new HashSet<ProductIngredient>();
        }
    
        public int IngredientID { get; set; }
        public string Title { get; set; }
        public string LongDescription { get; set; }
        public Nullable<double> Cost { get; set; }
        public Nullable<double> Weight { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> ImageID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartDetails> CartDetails { get; set; }
        public virtual Category Category { get; set; }
        public virtual Image Image { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductIngredient> ProductIngredient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductIngredient> ProductIngredient1 { get; set; }
    }
}