using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;

namespace CarRentalAPIBusinessLayer
{
    public class ClsCarCategory
    {
        public enum enMood { Add = 0, Update = 1 }

        public int? ID { get; set; }
        public string CategoryName { get; set; }
        public enMood Mood = enMood.Add;

        public CarCategoryDTO CDTO
        {
            get { return new CarCategoryDTO(this.ID, this.CategoryName); }
        }

        public ClsCarCategory(CarCategoryDTO CDTO, enMood Mood = enMood.Add)
        {
            this.ID = CDTO.Id;
            this.CategoryName = CDTO.CategoryName;
            this.Mood = Mood;
        }

        private bool AddNewCategory()
        {
            this.ID = ClsCarCategoryData.AddNewCategory(CDTO);
            return (this.ID != -1);
        }

        private bool _UpdateCategory()
        {
            return ClsCarCategoryData.UpdateCategory(CDTO);
        }

        public bool Save()
        {
            switch (Mood)
            {
                case enMood.Add:
                    if (AddNewCategory())
                    {
                        Mood = enMood.Update;
                        return true;
                    }
                    return false;

                case enMood.Update:
                    return _UpdateCategory();
            }
            return false;
        }

        public static bool Delete(int id)
        {
            return ClsCarCategoryData.Delete(id);
        }

        public static ClsCarCategory? GetById(int id)
        {
            CarCategoryDTO? dto = ClsCarCategoryData.GetById(id);
            if (dto == null) return null;
            return new ClsCarCategory(dto, enMood.Update);
        }

        public static List<ClsCarCategory> GetAllCategories()
        {
            List<ClsCarCategory> list = new List<ClsCarCategory>();
            List<CarCategoryDTO> dtos = ClsCarCategoryData.GetAllCategories();

            foreach (CarCategoryDTO dto in dtos)
                list.Add(new ClsCarCategory(dto, enMood.Update));

            return list;
        }
    }
}
