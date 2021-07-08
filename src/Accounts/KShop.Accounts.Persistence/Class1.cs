using System;
using System.Collections.Generic;

namespace KShop.Accounts.Persistence
{
    /* Данные адресов */
    /* Платежных карт */
    /*  */

    public class Account
    {
        public UserNameVO Name { get; set; }
        public DateTime BirthDate { get; set; }
        public EGender Gender { get; set; }

        public List<Address> Addresses { get; set; }
    }

    public enum EGender
    {
        None,
        Male,
        Female
    }

    public class UserNameVO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
    }

    public class Address
    {

    }

    public class CreditCard
    {

    }


}

