using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TARichardsonProject1.Models
{
    public class UserRepo
    {
        public int UserRepoID { get; set; }
        public User CurrentUser { get; set; }
        public Customer CurrentCustomer { get; set; }
        public List<Account> Accounts { get; set; }
        public List<TermAccount> TermAccounts { get; set; }
        public List<LoanAccount> LoanAccounts { get; set; }


    }
}