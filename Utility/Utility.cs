using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public enum State
    {
        WelcomePage,
        RegisterPage,
        LogInPage,
        CustomerPage,
        AccountPage,
        LogOutPage,
        SubmitRegisterPage,
        SubmitLoginPage,
        SubmitOpen,
        SubmitClose,
        OpenAccountPage,
        ListAccountPage,
        ListAccountTransactionPage,
        CloseCurrentAccountPage,
        CloseAccountPage,
        SelectAccountPage,
        WithDrawPage,
        DepositPage,
        TransferPage,
        UpdateAccountPage,
    };
    public enum AccountStatus
    {
        Active = 1,
        Inactive,
        Frozen,
    }
    public enum AccountType
    {
        CheckingAccount = 1,
        TermAccount,
        BusinessAccount,
        LoanAccount,
    };

    public enum TransactionType
    {
        WDW = 1,
        DPS,
        TRF,
        OPN,
        CLO,
        PLD,
        INR
    }


    public interface IRegLog
    {
        int Step { get; set; }

        void InputOne();
        void InputTwo();
        void ResetStep();

        void Print();
        State Submit();
    }
    public struct Register : IRegLog
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Step { get; set; }
        public Register(int step = 0)
        {
            FirstName = "";
            LastName = "";
            Step = step;
        }
        public void ResetValues()
        {
            FirstName = "";
            LastName = "";
        }
        public void InputOne()
        {
            Console.WriteLine("Enter your First Name:");
            FirstName = Console.ReadLine();
            Step += 1;
        }
        public void InputTwo()
        {
            Console.WriteLine("Enter your Last Name:");
            LastName = Console.ReadLine();
            Step += 1;
        }
        public void ResetStep()
        {
            Step = 0;
        }

        public void Print()
        {
            Console.WriteLine($"First Name: {FirstName} Last Name: {LastName}");

        }
        public State Submit()
        {
            return State.SubmitRegisterPage;
        }

    };
    public struct Login : IRegLog
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Step { get; set; }
        public Login(int step = 0)
        {
            UserName = "";
            Password = "";
            Step = step;
        }
        public void ResetValues()
        {
            UserName = "";
            Password = "";
        }
        public void InputOne()
        {
            Console.WriteLine("Enter your User Name:");
            UserName = Console.ReadLine();
            Step += 1;
        }
        public void InputTwo()
        {
            Console.WriteLine("Enter your Password:");
            Password = Console.ReadLine();
            Step += 1;
        }
        public void ResetStep()
        {
            Step = 0;
        }
        public void Print()
        {
            Console.WriteLine($"User Name: {UserName} Password: {Password}");

        }
        public State Submit()
        {
            return State.SubmitLoginPage;
        }
    };
    public struct OpenAccount : IRegLog
    {
        public float Balance { get; set; }
        public AccountType Type { get; set; }
        public int Step { get; set; }
        public OpenAccount(int step = 0, AccountType type = AccountType.CheckingAccount, float balance = 0)
        {
            Balance = balance;
            Type = type;
            Step = step;
        }

        public void InputOne()
        {
            Console.WriteLine("Enter your Account Type");
            Console.WriteLine("\t(0)CheckingAccount\n\t(1)TermAccount\n\t(2)BusinessAccount");
            try {
                int tempNum = Int32.Parse(Console.ReadLine());

                if (tempNum > -1 && tempNum < 3)
                {
                    Type = (AccountType)Enum.Parse(typeof(AccountType), tempNum.ToString(), true);
                    Step += 1;
                }
                else
                {
                    Console.WriteLine("invalid input");

                }
            }catch(Exception)
            {
                Console.WriteLine("invalid input");
            }

        }
        public void InputTwo()
        {
            Console.WriteLine("Enter your Starting Balance:");
            Balance = float.Parse(Console.ReadLine());
            Step += 1;
        }
        public void ResetStep()
        {
            Step = 0;
        }
        public void ResetValues(AccountType type = AccountType.CheckingAccount, float balance = 0)
        {
            Balance = balance;
            Type = type;
        }
        public void Print()
        {
            Console.WriteLine($"Account Type: {Type} Starting Balance: {Balance}");

        }
        public State Submit()
        {
           return State.SubmitOpen;
        }
    }

}
