using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Vidly.Models
{

    [Table("MVCCustomers")]
    public class Customer
    {

        //[DatabaseGenerated(DatabaseGeneratedOption.None)] // I put this in - as adding a new customer - I pass in the Id field. And if you don't add this attribute, it will always pass NULL to the db hoping that the db will seed.
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key] // don't technically need this is the field is called 'Id'
        public Guid Id { get; set; }

        // These attributes will allow you to use  @Html.TextBoxFor(m => m.Name, new { @class = "form-control" } ) - and it will do all your
        // validation for you.
        [Required(ErrorMessage = "You must fill in the name")] //Had to add using DataAnnotations to make this work. Added 'ErrorMessage' if you want to override default message.
        [StringLength(255)]
        [Display(Name = "Customer Name")]
        [MustNotContainHAYAsExampleCustomAtt()]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Membership Type ID")]
        public int MembershipType { get; set; }
    }
}