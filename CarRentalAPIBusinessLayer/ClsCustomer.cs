using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;

namespace CarRentalAPIBusinessLayer
{
    public class ClsCustomer
    {
        public enum enMood { Add = 0, update = 1 }

        public int? ID { set; get; }
        public string Name { set; get; }
        public string ContactInformation { set; get; }
        public string DriverLicenseNumber { set; get; }
        public enMood Mood = enMood.Add;

        public CustomerDTO CDTO
        {
            get { return new CustomerDTO(this.ID, this.Name, this.ContactInformation, this.DriverLicenseNumber); }
        }

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

        private bool _UpdateCustomer()
        {
            return ClsCustomerData.UpdateCustomer(CDTO);
        }

        public bool Save()
        {
            switch (Mood)
            {
                case enMood.Add:
                    if (AddNewCustomer())
                    {
                        Mood = enMood.update;
                        return true;
                    }
                    return false;

                case enMood.update:
                    return _UpdateCustomer();
            }
            return false;
        }

        
        public static bool Delete(int id)
        {
            return ClsCustomerData.Delete(id);
        }

        
        public static ClsCustomer? GetById(int id)
        {
            CustomerDTO? dto = ClsCustomerData.GetById(id);
            if (dto == null) return null;
            return new ClsCustomer(dto, enMood.update);
        }

        
        public static List<ClsCustomer> GetAllCustomers()
        {
            List<ClsCustomer> list = new List<ClsCustomer>();
            List<CustomerDTO> dtos = ClsCustomerData.GetAllCustomers();

            foreach (CustomerDTO dto in dtos)
                list.Add(new ClsCustomer(dto, enMood.update));

            return list;
        }

        public static List<ClsCustomer> GetByName(string name)
        {
            List<ClsCustomer> list = new List<ClsCustomer>();
            List<CustomerDTO> dtos = ClsCustomerData.GetByName(name);

            foreach (CustomerDTO dto in dtos)
                list.Add(new ClsCustomer(dto, enMood.update));

            return list;
        }
    }
}
