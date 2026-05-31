using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq; 
using Model.Core;

namespace Model.Data

{
    // "shelters_database.json" желательно удалять помле каждого запуска чтобы данные были изначальные например когда бы добав или улал животного +1 -1 и тд
    public class JsonSerializer : Serializer
    {
        public override string Extension => ".json";

        //Запись всего списка приютов в файл JSON
       
        public override void Serialize(string folderPath, List<Shelter> data)
        {
            if (data == null) return;

            string filePATH = Path.Combine(folderPath, "shelters_database.json");
            JArray sheltersList = new JArray();

            foreach (Shelter shelter in data)
            {
                JObject shelterBlock = new JObject
                {
                    { "Name", shelter.Name },
                    { "Capacity", shelter.Capacity },
                    { "HasOpenTerritory", shelter.HasOpenTerritory }
                };

                JArray petsList = new JArray();
                foreach (Pet pet in shelter.Pets)
                {
                    if (pet == null) continue;

                    JObject petBlock = new JObject
                    {
                        { "Name", pet.Name },
                        { "Age", pet.Age },
                        { "Weight", pet.Weight },
                        { "Height", pet.Height },
                        { "IsClaustrophobic", pet.IsClaustrophobic },
                        { "Type", pet.GetType().Name }
                    };

                    if (pet is Cat cat)
                    {
                        petBlock.Add("FurColor", cat.FurColor);
                        petBlock.Add("IsLazy", cat.IsLazy);
                    }
                    else if (pet is Dog dog)
                    {
                        petBlock.Add("Breed", dog.Breed);
                        petBlock.Add("KnowsCommands", dog.KnowsCommands);
                    }
                    else if (pet is Rabbit rabbit)
                    {
                        petBlock.Add("EarLength", rabbit.EarLength);
                        petBlock.Add("IsDomestic", rabbit.IsDomestic);
                    }
                    else if (pet is Parrot parrot)
                    {
                        petBlock.Add("Gender", parrot.Gender);
                        petBlock.Add("IsTalking", parrot.IsTalking);
                    }

                    petsList.Add(petBlock);
                }

                shelterBlock.Add("Pets", petsList);
                sheltersList.Add(shelterBlock);
            }

            File.WriteAllText(filePATH, sheltersList.ToString());
        }

       
        //Чтение из файла JSON 
       
        public override List<Shelter> Deserialize(string folderPath)
        {
            List<Shelter> shelters = new List<Shelter>();
            string filePATH = Path.Combine(folderPath, "shelters_database.json");

            if (!File.Exists(filePATH)) return shelters;

            string jsonText = File.ReadAllText(filePATH);
            JArray sheltersList = JArray.Parse(jsonText);

            foreach (JObject shelterBlock in sheltersList)
            {
                string sName = shelterBlock["Name"].ToString();
                int sCapacity = int.Parse(shelterBlock["Capacity"].ToString());
                bool sHasOpen = bool.Parse(shelterBlock["HasOpenTerritory"].ToString());

                Shelter shelter = new Shelter(sName, sCapacity, sHasOpen);

                JArray petsList = (JArray)shelterBlock["Pets"];
                if (petsList != null)
                {
                    foreach (JObject petBlock in petsList)
                    {
                        string name = petBlock["Name"].ToString();
                        int age = int.Parse(petBlock["Age"].ToString());
                        double weight = double.Parse(petBlock["Weight"].ToString());
                        double height = double.Parse(petBlock["Height"].ToString());
                        bool isClaustrophobic = bool.Parse(petBlock["IsClaustrophobic"].ToString());
                        string type = petBlock["Type"].ToString();

                        if (type == "Cat")
                        {
                            string color = petBlock["FurColor"].ToString();
                            bool isLazy = bool.Parse(petBlock["IsLazy"].ToString());
                            shelter.Pets.Add(new Cat(name, age, weight, height, color, isLazy, isClaustrophobic));
                        }
                        else if (type == "Dog")
                        {
                            string breed = petBlock["Breed"].ToString();
                            bool knowsCommands = bool.Parse(petBlock["KnowsCommands"].ToString());
                            shelter.Pets.Add(new Dog(name, age, weight, height, breed, knowsCommands, isClaustrophobic));
                        }
                        else if (type == "Rabbit")
                        {
                            double earLength = double.Parse(petBlock["EarLength"].ToString());
                            bool isDomestic = bool.Parse(petBlock["IsDomestic"].ToString());
                            shelter.Pets.Add(new Rabbit(name, age, weight, height, earLength, isDomestic, isClaustrophobic));
                        }
                        else if (type == "Parrot")
                        {
                            string gender = petBlock["Gender"].ToString();
                            bool isTalking = bool.Parse(petBlock["IsTalking"].ToString());
                            shelter.Pets.Add(new Parrot(name, age, weight, height, gender, isTalking, isClaustrophobic));
                        }
                    }
                }

                shelters.Add(shelter);
            }

            return shelters;
        }

        
        // СЕРИАЛИЗАЦИЯ
       
        public override void SerializeSelection(string folderPath, List<Pet> pets)
        {
            if (pets == null) return;

            
            string uniqueNumber = Environment.TickCount.ToString();
            string filePATH = Path.Combine(folderPath, "Подборка_" + uniqueNumber + ".json");

            JArray petsList = new JArray();

            foreach (Pet pet in pets)
            {
                if (pet == null) continue;
                JObject petBlock = new JObject
                {
                    { "Name", pet.Name },
                    { "Age", pet.Age },
                    { "Weight", pet.Weight },
                    { "Height", pet.Height },
                    { "Type", pet.GetType().Name }
                };
                petsList.Add(petBlock);
            }

            File.WriteAllText(filePATH, petsList.ToString());
        }
    }
}
