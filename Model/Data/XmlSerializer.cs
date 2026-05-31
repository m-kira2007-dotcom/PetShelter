using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization; 
using Model.Core;

namespace Model.Data
{
    //"shelters_database.xml" желательно удалять после запуска чтобы данные были изначальные
    public class XmlSerializer : Serializer
    {
        // Возвращает расширение файла для нашего главного менеджера AllInformation
        public override string Extension => ".xml";

        
        //  СЕРИАЛИЗАЦИЯ (Оригинальный список приютов -> DTO -> Файл XML)
        
        public override void Serialize(string folderPath, List<Shelter> data)
        {
            if (data == null) return;

            // Склеиваем путь до папки дебага и имя файла "shelters_database.xml" 
            string filePATH = Path.Combine(folderPath, "shelters_database.xml");

            // Переводим оригинальные данные в DTO формат 
            ShelterContainerDTO containerDTO = new ShelterContainerDTO();
            foreach (var shelter in data)
            {
                containerDTO.Shelters.Add(new ShelterDTO(shelter));
            }

            //  Создаем XML-сериализатор передавая ему тип нашего DTO-контейнера
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ShelterContainerDTO));

            //  Используем блок using и StreamWriter для безопасной записи файла, как в вашем примере
            using (var writer = new StreamWriter(filePATH))
            {
                serializer.Serialize(writer, containerDTO);
            }
        }

        //  ДЕСЕРИАЛИЗАЦИЯ (Файл XML -> DTO объект -> Оригинальный List)
       
        public override List<Shelter> Deserialize(string folderPath)
        {
            string filePATH = Path.Combine(folderPath, "shelters_database.xml");

            // Если файла на диске еще нет — возвращаем пустой список, программа не упадет
            if (!File.Exists(filePATH)) return new List<Shelter>();

            //  Создаем сериализатор для чтения
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(ShelterContainerDTO));
            ShelterContainerDTO containerDTO;

            //  Читаем DTO-объект из XML-файла через StreamReader внутри using
            using (var reader = new StreamReader(filePATH))
            {
                containerDTO = (ShelterContainerDTO)serializer.Deserialize(reader);
            }

            //  Восстанавливаем оригинальные объекты из DTO ( по полю Type)
            List<Shelter> originalShelters = new List<Shelter>();

            foreach (var sDto in containerDTO.Shelters)
            {
                // Создаем оригинальный объект приюта
                Shelter originalShelter = new Shelter(sDto.Name, sDto.Capacity, sDto.HasOpenTerritory);

                foreach (var pDto in sDto.Pets)
                {
                    // В зависимости от того, какой Type записан в DTO, создаем нужный классн аследник Pet
                    if (pDto.Type == "Cat")
                    {
                        
                        originalShelter.Pets.Add(new Cat(pDto.Name, pDto.Age, pDto.Weight, pDto.Height, pDto.FurColor, pDto.IsLazy, pDto.IsClaustrophobic));
                    }
                    else if (pDto.Type == "Dog")
                    {
                        originalShelter.Pets.Add(new Dog(pDto.Name, pDto.Age, pDto.Weight, pDto.Height, pDto.Breed, pDto.KnowsCommands, pDto.IsClaustrophobic));
                    }
                    else if (pDto.Type == "Rabbit")
                    {
                        originalShelter.Pets.Add(new Rabbit(pDto.Name, pDto.Age, pDto.Weight, pDto.Height, pDto.EarLength, pDto.IsDomestic, pDto.IsClaustrophobic));
                    }
                    else if (pDto.Type == "Parrot")
                    {
                        
                        originalShelter.Pets.Add(new Parrot(pDto.Name, pDto.Age, pDto.Weight, pDto.Height, pDto.Gender, pDto.IsTalking, pDto.IsClaustrophobic));
                    }

                }

                originalShelters.Add(originalShelter);
            }

            return originalShelters;
        }
        // СЕРИАЛИЗАЦИЯ(сохранение отчетов из окон таблиц WPF)
        public override void SerializeSelection(string folderPath, List<Pet> pets)
        {
            if (pets == null) return;

            // Генерируем уникальное имя файла подборки
            string filePATH = Path.Combine(folderPath, $"Подборка_№{new Random().Next(1, 100)}_от_{DateTime.Now:dd_MM_yyyy_HHmmss}.xml");

            // Конвертируем отфильтрованный список животных в список плоских DTO
            List<PetDTO> dtoList = new List<PetDTO>();
            foreach (var pet in pets)
            {
                if (pet != null) dtoList.Add(new PetDTO(pet));
            }

            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<PetDTO>));

            using (var writer = new StreamWriter(filePATH))
            {
                serializer.Serialize(writer, dtoList);
            }
        }
    }
}
