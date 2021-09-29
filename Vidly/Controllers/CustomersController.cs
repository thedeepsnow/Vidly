using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {

        // THIS USES CODE-FIRST DATABASE DESIGN - SO YOU CAN'T MAKE CHANGES TO THE DATABASE FROM MANAGEMENT STUDIO.
        // IF YOU CHANGE THE CONNECTION STRING IN WEB.CONFIG - IT WILL CREATE THE TABLE COMPLETELY FOR YOU IN THE NEW DATABASE.
        // IT WILL CREATE TABLES WITH 'S' ON THE END UNLESS YOU ADD THE TABLE TAG TO YOUR DB MODEL CLASS - E.G. [Table("MVCCustomers")]

        // AT THIS STAGE - TO CHANGE A COLUMN - COMPLETELY DELETE THE TABLE FROM THE DATABASE AND THE _MigrationHistory table which it might have created.

        public class MovieDBContext : DbContext
        {
            public MovieDBContext()
            {
                Debug.WriteLine("Generated connection string: " + Database.Connection.ConnectionString);
                Database.Connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings
                    ["MovieDBContext"].ConnectionString;
                Debug.WriteLine("Manually set connection string: " + Database.Connection.ConnectionString);
            }

            public DbSet<Customer> MVCCustomers { get; set; }
        }

        public List<Models.MembershipType> loadMembershipTypes()
        {
            List<Models.MembershipType> lstMembershipTypes = new List<MembershipType>();
            
            lstMembershipTypes.Add(new MembershipType() {id = 1, Description = "Member Type 1"} );
            lstMembershipTypes.Add(new MembershipType() {id = 2, Description = "Member Type 2"} );

            return lstMembershipTypes;
        }

        public List<Models.Customer> LoadCustomers()
        {
            List<Models.Customer> lstCustomers = new List<Models.Customer>();

            //using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings
            //        ["MovieDBContext"].ConnectionString))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Customers", con))
            //    {
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);

            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            Customer cust = new Customer();
            //            cust.Id = (int)dr["Id"];
            //            cust.Name = dr["Name"].ToString();
            //            cust.MembershipType = (int)dr["MembershipType"];
            //            lstCustomers.Add(cust);
            //        }
            //        //lstCustomers = dt.Select().ToList();
            //    }
            //}

            using (MovieDBContext db = new MovieDBContext())
            {
                lstCustomers = db.MVCCustomers.ToList();
            }

            return lstCustomers;
        }

        // GET: Customers
        [Route("customers/index")]
        public ActionResult Index()
        {
            try
            {
                ViewModels.CustomersViewModel cust = new ViewModels.CustomersViewModel();
                cust.Customers = LoadCustomers();

                return View(cust);
            }
            catch (Exception ex)
            {
                return Content(ex.ToString());
            }
        }

        [Route("customers/details")]
        public ActionResult Details(Guid Id)
        {
            Models.Customer iCustomer = new Models.Customer();
            iCustomer = LoadCustomers().Find(x => x.Id == Id);
           
            return View(iCustomer);
        }

        // called in Views -> Customers -> index.cshtml
        public ActionResult New()
        {
            // pass in the membership types for the drop down list.
            ViewModels.CustomerFormViewModel vm = new ViewModels.CustomerFormViewModel();
            vm.lstMembershipTypes = loadMembershipTypes();
            vm.Customer = new Customer();

            //we don't need that first parameter - but if we renamed the view, we would need it.
            return View("CustomerForm", vm);
        }

        [HttpPost] // make sure it can only be called by a post of data and not a get from the user.
        [ValidateAntiForgeryToken] // must include this - see note at bottom of CustomerForm.cshtml 'AntiForgeryToken'
        public ActionResult Save(CustomerFormViewModel vm)
        {
            // for this 'NewCustomerViewModel vm' to 'model binding' with the form itself, you must have the get; set; on each model / model view variable.
            //Debug.WriteLine("Name: " + vm.Customer.Name);

            //using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings
            //        ["MovieDBContext"].ConnectionString))
            //{
            //    using (SqlDataAdapter da = new SqlDataAdapter("SELECT TOP 1 * FROM Customers ORDER BY Id DESC", con))
            //    {
            //        SqlCommandBuilder cb = new SqlCommandBuilder(da);
            //        DataTable dt = new DataTable();
            //        da.Fill(dt);

            //        int newid = (int)dt.Rows[0]["Id"] + 1;

            //        DataRow nr;
            //        nr = dt.NewRow();
            //        nr["Name"] = vm.Customer.Name;
            //        nr["MembershipType"] = vm.Customer.MembershipType;
            //        nr["Id"] = newid;

            //        dt.Rows.Add(nr);
            //        da.Update(dt);
            //    }
            //}

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors); // can debug on this line if unsure why not valid.
                vm.lstMembershipTypes = loadMembershipTypes(); // it seems to only send through submitted data in the vm, so re-populate to send back.
                return View("CustomerForm", vm);
            }

            using (MovieDBContext db = new MovieDBContext())
            {
                if (db.MVCCustomers.Any(x => x.Id == vm.Customer.Id)) // new customer
                {
                    // use Single rather than SingleOrDefault so that throws exception if not found.
                    Customer cust = db.MVCCustomers.Single(x => x.Id == vm.Customer.Id);

                    //recommended not to use this as request variables could be hacked.
                    //TryUpdateModel(cust); // will update based on request variables into the action.

                    //so ...

                    cust.Name = vm.Customer.Name;
                    cust.MembershipType = vm.Customer.MembershipType;
                }
                else // existing customer
                {

                    db.MVCCustomers.Add(vm.Customer);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        } // end of Create

        // called in Views -> Customers -> index.cshtml
        public ActionResult Edit(Guid Id)
        {
            //Console.WriteLine("Name: " + vm.Customer.Name);

            using (MovieDBContext db = new MovieDBContext())
            {
                Customer cust = db.MVCCustomers.SingleOrDefault(x => x.Id == Id);

                if (cust == null) { return HttpNotFound(); }

                CustomerFormViewModel newCust = new CustomerFormViewModel();
                newCust.Customer = cust;
                newCust.lstMembershipTypes = loadMembershipTypes();

                return View("CustomerForm", newCust);
            }
        } // end of Edit

    } // end of Class

} // end of Namespace
