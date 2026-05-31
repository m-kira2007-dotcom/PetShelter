using System;
using System.Collections.Generic;

namespace Model.Core
{
    // ОБЩИЙ DTO КЛАСС ДЛЯ ЖИВОТНЫХ для xml
    public class PetDTO
    {
        // Базовые свойства
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
        public bool IsClaustrophobic { get; set; }

        // Системное поле для определения вида (Cat, Dog, Rabbit, Parrot)
        public string Type { get; set; }

        // Уникальные поля разных видов (будут заполняться выборочно)
        public bool IsLazy { get; set; }
        public string FurColor { get; set; }
        public string Breed { get; set; }
        public bool KnowsCommands { get; set; }
        public double EarLength { get; set; }
        public bool IsDomestic { get; set; }
        public bool IsTalking { get; set; }
        public string Gender { get; set; }

        // Пустой конструктор для XML-сериализатора (Обязателенo
        public PetDTO() 
        {

        }

        // Конструктор, принимающий оригинальный объект Pet 
        public PetDTO(Pet pet)
        {
            Name = pet.Name;
            Age = pet.Age;
            Weight = pet.Weight;
            Height = pet.Height;
            IsClaustrophobic = pet.IsClaustrophobic;
            Type = pet.GetType().Name; // Записываем имя класса (собака кошка поп

            // Проверяем точный вид и дозаписываем уникальные характеристики
            if (pet is Cat cat)
            {
                IsLazy = cat.IsLazy;
                FurColor = cat.FurColor;
            }
            else if (pet is Dog dog)
            {
                Breed = dog.Breed;
                KnowsCommands = dog.KnowsCommands;
            }
            else if (pet is Rabbit rabbit)
            {
                EarLength = rabbit.EarLength;
                IsDomestic = rabbit.IsDomestic;
            }
            else if (pet is Parrot parrot)
            {
                IsTalking = parrot.IsTalking;
                Gender = parrot.Gender;
            }
        }
    }
    //DTO КЛАСС ДЛЯ ПРИЮТА 

    public class ShelterDTO
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public bool HasOpenTerritory { get; set; }

        // Список питомцев переведен в формат PetDTO
        public List<PetDTO> Pets { get; set; } = new List<PetDTO>();

        public ShelterDTO() { }

        // Конструктор принимает оригинальный приют и переводит его данные в DTO
        public ShelterDTO(Shelter shelter)
        {
            Name = shelter.Name;
            Capacity = shelter.Capacity;
            HasOpenTerritory = shelter.HasOpenTerritory;

            foreach (var pet in shelter.Pets)
            {
                if (pet != null)
                {
                    Pets.Add(new PetDTO(pet)); // Конвертируем каждого питомца в DTO
                }
            }
        }
    }
    //   ДЛЯ СПИСКА ПРИЮТОВ
    public class ShelterContainerDTO
    {
        public List<ShelterDTO> Shelters { get; set; } = new List<ShelterDTO>();
    }
}
