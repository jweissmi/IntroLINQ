using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;


namespace IntroLINQ
{
    class Program
    {
        static void Main(string[] args)
        {

            NorthwindDataContext db = new NorthwindDataContext();

            if (!db.DatabaseExists())
                throw new Exception();

            //LINQ query to select everything in table.
            var allCusts = db.Customers;

            foreach (var customer in allCusts)
            {
                Console.WriteLine("{0},   {1}", customer.CompanyName, customer.ContactName);
            }
            Console.WriteLine();
            Console.WriteLine();

            //LINQ query to filter the result set based on the values in one of the columns.
            var someCusts = from customer in db.Customers
                            where customer.Region == "BC"
                            select customer;

            foreach (var customer in someCusts)
            {
                Console.WriteLine("{0},   {1}   {2}", customer.CompanyName, customer.ContactName, customer.Region);
            }
            Console.WriteLine();
            Console.WriteLine();

            //LINQ query to Order the results by the value of one of the columns(ascending and descending).
            //LINQ queries that will filter the result set by 2 or more columns in the data (using && and ||).
            var otherCusts = from customer in db.Customers
                             where customer.Country == "USA" || customer.Country == "Canada"
                             orderby customer.CompanyName ascending
                             select customer;

            foreach (var customer in otherCusts)
            {
                Console.WriteLine("{0},   {1}   {2}", customer.CompanyName, customer.ContactName, customer.Country);
            }
            Console.WriteLine();
            Console.WriteLine();

            //LINQ query that will group the result
            var custQuery4 = from customer in db.Customers
                             group customer by customer.CompanyName[0] into customerGroup
                             orderby customerGroup.Key
                             select customerGroup;

            foreach (var groupOfCustomers in custQuery4)
            {
                Console.WriteLine(groupOfCustomers.Key);
                foreach (var customer in groupOfCustomers)
                {
                    Console.WriteLine(" {0},   {1}", customer.CompanyName, customer.Country);
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            var innerJoin = from t in db.Territories
                            join r in db.Regions on t.RegionID equals r.RegionID
                            select new { Terr = t.TerritoryDescription, Reg = r.RegionDescription };

            //The above is an anonymous type.  It is a class with terr get set, reg with get set, properties of string
            //Inner join equals IEnumerable of terr & reg

            foreach (var tempClass in innerJoin)

                Console.WriteLine("Territory Name : {0}              Region Name : {1}",
                    (tempClass.Terr).Trim(), (tempClass.Reg).Trim());

            Console.WriteLine();
            Console.WriteLine();

            var peopleInLondon = (from c in db.Customers
                                  where c.City == "London"
                                  select c.ContactName)
                                  .Concat(from e in db.Employees
                                          where e.City == "London"
                                          select e.FirstName + " " + e.LastName);


            // use Where() to find only elements that match
            IEnumerable<Customer> q = db.Customers.Where(c => c.City == "London");
            Console.WriteLine("People in London", q);
            foreach (var peeps in peopleInLondon)

                Console.WriteLine("Name : {0}", peeps);

            Console.WriteLine();
            Console.WriteLine();

            Console.ReadLine();
        }
    }
}
