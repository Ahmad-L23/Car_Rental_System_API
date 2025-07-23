using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class ClsCustomer
    {
        public enum enMood { Add = 0, update = 1}

        public int? ID { set; get; }
        public string Name { set; get; }
        public string ContactInformation { set; get; }
        public string DriverLicenseNumber { set; get; }
        public enMood Mood =enMood.Add;

        public CustomerDTO CDTO { get { return ( new CustomerDTO(this.ID, this.Name, this.ContactInformation, this.DriverLicenseNumber) ); } }

        public ClsCustomer(CustomerDTO CDTO, enMood Mood = enMood.Add)
        {
            this.ID = CDTO.Id;
            this.Name = CDTO.Name;
            this.ContactInformation = CDTO.ContactInformation;
            this.DriverLicenseNumber = CDTO.DriverLicenseNumber;
            this.Mood = Mood;
        }
        

        private bool AddNewCustomer()
        {
            this.ID = ClsCustomerData.AddNewCustomer(CDTO);
            
            return (this.ID != -1);
        }


        public bool Save()
        {
            switch (Mood)
            {
                case enMood.Add:
                    AddNewCustomer();
                    Mood = enMood.update;
                    return true;

            }
            return false;
        }

    }
}
