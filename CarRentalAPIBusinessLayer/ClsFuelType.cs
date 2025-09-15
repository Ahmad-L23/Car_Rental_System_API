using CarRentalDataAccessLayer;
using System;
using System.Collections.Generic;

namespace CarRentalAPIBusinessLayer
{
    public class ClsFuelType
    {
        public enum enMood { Add = 0, Update = 1 }

        public int? ID { get; set; }
        public string FuelType { get; set; }
        public enMood Mood = enMood.Add;

        public FuelTypeDTO DTO
        {
            get { return new FuelTypeDTO(this.ID, this.FuelType); }
        }

        public ClsFuelType(FuelTypeDTO dto, enMood Mood = enMood.Add)
        {
            this.ID = dto.Id;
            this.FuelType = dto.FuelType;
            this.Mood = Mood;
        }

        private bool AddNewFuelType()
        {
            this.ID = ClsCarFuelTypeData.AddNewFuelType(DTO);
            return (this.ID != -1);
        }

        private bool _UpdateFuelType()
        {
            return ClsCarFuelTypeData.UpdateFuelType(DTO);
        }

        public bool Save()
        {
            switch (Mood)
            {
                case enMood.Add:
                    if (AddNewFuelType())
                    {
                        Mood = enMood.Update;
                        return true;
                    }
                    return false;

                case enMood.Update:
                    return _UpdateFuelType();
            }
            return false;
        }

        public static bool Delete(int id)
        {
            return ClsCarFuelTypeData.Delete(id);
        }

        public static ClsFuelType? GetById(int id)
        {
            FuelTypeDTO? dto = ClsCarFuelTypeData.GetById(id);
            if (dto == null) return null;
            return new ClsFuelType(dto, enMood.Update);
        }

        public static List<ClsFuelType> GetAllFuelTypes()
        {
            List<ClsFuelType> list = new List<ClsFuelType>();
            List<FuelTypeDTO> dtos = ClsCarFuelTypeData.GetAllFuelTypes();

            foreach (var dto in dtos)
                list.Add(new ClsFuelType(dto, enMood.Update));

            return list;
        }
    }
}
