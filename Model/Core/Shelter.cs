using Model.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core
{
    public partial class Shelter : ICountable, IFilter,IChangeable
    {
        public string Name { get; set; }
        public int Capacity { get; set; } // вместимость 
        public bool HasOpenTerritory { get; set; } // сколько открытой территори

        // инициализация через классический пустой массив по умолчанию
        public List<Pet> Pets { get; set; } = new List<Pet>();

        public static Func<Pet, bool> CheckIfPetIsAllowed;

        public Shelter()
        {

        }

        public Shelter(string name, int capacity, bool hasOpenTerritory)
        {
            Name = name;
            Capacity = capacity;
            HasOpenTerritory = hasOpenTerritory;
        }
        //КОД ПЕРЕГРУЗКИ ОПЕРАТОРОВ 

        // Позволяет сравнивать два приюта по количеству животных в них
        public static bool operator >(Shelter first, Shelter second)
        {
            if (first == null || second == null) return false;
            return first.Count() > second.Count();
        }
        public static bool operator <(Shelter first, Shelter second)
        {
            if (first == null || second == null) return false;
            return first.Count() < second.Count();
        }

        //интерфейса ICountable
        public int Count()
        {
            return Pets.Count; // сколько животных в приюте
        }

        public int Count(Type type)
        {
            if (type == null) return Pets.Count;

            int count = 0;
            foreach (var pet in Pets)
            {
                if (pet != null && pet.GetType() == type)
                {
                    count++;
                }
            }
            return count;
        }

        public int Percentage(Type type)
        {
            if (Pets.Count == 0 || type == null) return 0;

            double countOfType = Count(type);
            double totalCount = Pets.Count;

            int procent=(int)Math.Round(countOfType / totalCount * 100);
            return procent;
        }
        
        public List<Pet> Filter(Type type) 
        {
            //  Создаем пустой динамический список, который будем возвращать
            List<Pet> result = new List<Pet>();

            //  Если тип не указан (выбрано "Все виды" -> null), 
            // переносим абсолютно всех живых питомцев из массива в этот список
            if (type == null)
            {
                foreach (var pet in Pets)
                {
                    if (pet != null)
                    {
                        result.Add(pet);
                    }
                }
                return result;
            }

            //  Если тип указан, бежим циклом по массиву Pets 
            // и добавляем в список только тех, чей тип совпал (например, Cat)
            foreach (var pet in Pets)
            {
                if (pet != null && pet.GetType() == type)
                {
                    result.Add(pet); // Метод Add сам расширяет список
                }
            }

            // Возвращаем заполненный список
            return result;
        }
        //Перегрузка метода фильтрации
        public List<Pet> Filter(Type type, bool checkClaustrophobia)
        {
            List<Pet> baseFiltered = Filter(type);
            if (!checkClaustrophobia) return baseFiltered;

            List<Pet> finalResult = new List<Pet>();
            foreach (var pet in baseFiltered)
            {
                if (pet != null)
                {
                    // Животных с клаустрофобией нельзя помещать в закрытый питомник
                    if (!this.HasOpenTerritory && pet.IsClaustrophobic)
                    {
                        continue;
                    }
                    finalResult.Add(pet);
                }
            }
            return finalResult;
        }


        //Реализация интерфейса IChangeable 

        public bool AddPet(Pet pet)
        {
            if (pet == null) return false;

            if (Pets.Count >= Capacity) return false;

            if (!HasOpenTerritory && pet.IsClaustrophobic) return false;
            // Если проверка 'Разрешено ли животное' вернула ложь -> выход
            if (CheckIfPetIsAllowed != null && CheckIfPetIsAllowed(pet) == false)
            {
                return false;
            }

            Pets.Add(pet);
            return true;
        }


        public void RemovePet(Pet pet)
        {
            if (pet != null && Pets.Contains(pet))
            {
                Pets.Remove(pet);
            }
        }
    }
}

