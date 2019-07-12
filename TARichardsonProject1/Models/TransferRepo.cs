using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARichardsonProject1.Models
{
    public class TransferRepo
    {
        public int TransRepoID { get; set; }
        public decimal Sum { get; set; }
        public User CurrentUser { get; set; }
        public Customer CurrentCustomer { get; set; }
        public Account FromAccount { get; set; }
        public Account ToAccount { get; set; }

    }
}