//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BookManagementAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommentID { get; set; }
        public int BookID { get; set; }
        public string CommentContent { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Book Book { get; set; }
    }
}
