using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.ViewModels
{
    public class CustomerFormViewModel
    {
        public List<Models.MembershipType> lstMembershipTypes { get; set; }
        public Models.Customer Customer { get; set; }
    }
}