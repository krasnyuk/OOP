//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MemberRMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetails
    {
        public int DetailsID { get; set; }
        public Nullable<int> OrderID { get; set; }
        public Nullable<int> Amount { get; set; }
    
        public virtual Order Order { get; set; }
    }
}
