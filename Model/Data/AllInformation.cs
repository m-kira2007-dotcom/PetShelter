using Model.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public static class AllInformation
    {
        private const string BaseFileName = "shelters_database";

       
        public static Serializer CurrentSerializer { get; set; } = new JsonSerializer();

        
        //  Главный метод загрузки или автоматического создания базы данных
        
        public static List<Shelter> LoadOrCreateData(string folderPath)
        {
            // Склеиваем путь к папке дебага и имя файла с расширением (.json или .xml)
            string path = Path.Combine(folderPath, BaseFileName + CurrentSerializer.Extension);

            // Если программа запускается второй раз — берем данные с диска (Десериализация)
            if (File.Exists(path))
            {
                return CurrentSerializer.Deserialize(folderPath);
            }

            // Если запускается САМЫЙ ПЕРВЫЙ РАЗ — генерируем  список через List
            List<Shelter> defaultData = CreateMockData();

          
            new JsonSerializer().Serialize(folderPath, defaultData);
            new XmlSerializer().Serialize(folderPath, defaultData);

            return defaultData;
        }

        // Метод сохранения базы при изменении состава животных (для интерфейса IChangeable)
        public static void SaveData(string folderPath, List<Shelter> data)
        {
            CurrentSerializer.Serialize(folderPath, data);
        }

        // Метод  Копирование данных отчетов при смене формата в меню
        public static void ConvertReports(string folderPath, Serializer oldSerializer, Serializer newSerializer)
        {
            //  Если папки нет на диске — сразу выходим
            if (!Directory.Exists(folderPath)) return;

            //  Ищем все файл которые начинаются со слова "Подборка_" и имеют старое расширение (.json)
            string searchPattern = "Подборка_*" + oldSerializer.Extension;
            string[] oldFiles = Directory.GetFiles(folderPath, searchPattern);

            //  Бежим циклом по найденным старым файлам подборок
            foreach (string oldFilePath in oldFiles)
            {
                // Формируем новый путь, меняя расширение (например, .json на .xml)
                string newFilePath = Path.ChangeExtension(oldFilePath, newSerializer.Extension);

                // Если такого файла в новом формате еще нет — создаем его текстовый дубликат
                if (!File.Exists(newFilePath))
                {
                    string content = File.ReadAllText(oldFilePath);
                    File.WriteAllText(newFilePath, content);
                }
            }
        }
        //база данных приютов и животных через List в нужном порядке
        private static List<Shelter> CreateMockData()
        {
            List<Shelter> shelters = new List<Shelter>
            {
                new Shelter("ADOPT ME", 15, false), // Закрытый приют
                new Shelter("В добрые руки", 18, true),       // Открытый приют
                new Shelter("Усики да хвостики", 25, true)        // Открытый приют
            };

            // Наполнение приюта 1 
            shelters[0].Pets.AddRange(new List<Pet>
            {
                // Порядок Cat: (name, age, weight, height, color, isLazy, isClaustrophobic)
                new Cat("Lucky", 3, 4.5, 25.0, "коричнево-черный", true, isClaustrophobic: false),
                new Cat("Буся", 2, 5.0, 27.0, "Серый", false, isClaustrophobic: true),
                // Порядок Rabbit: (name, age, weight, height, earLength, isDomestic, isClaustrophobic)
                // Кролик Снежок: true по умолчанию, автоматически скроется в закрытом приюте!
                new Rabbit("Коксик", 1, 1.2, 18.0, 10.5, true),
                //  public Parrot(string name, int age, double weight, double height, string gender, bool isTalking, bool isClaustrophobic = false) 
                new Parrot("Попка", 2, 2, 21.0, "Мужской", true, isClaustrophobic: true)
            });

            // Наполнение приюта 2 
            shelters[1].Pets.AddRange(new List<Pet>
            {
                // Порядок Dog: (name, age, weight, height, breed, knowsCommands, isClaustrophobic)
                new Dog("Петя", 1, 12.5, 55.0, "Бульдог", false, isClaustrophobic: false),
                new Parrot("Лиза", 2, 3, 24.0, "Женский", true, isClaustrophobic: false),
                new Dog("Жанна", 7, 15.0, 48.0, "Пудель", true, isClaustrophobic: false),
                new Cat("Симба", 1, 2.5, 15.0, "Рыжий", false, isClaustrophobic: true)
            });

            // Наполнение приюта 3
            shelters[2].Pets.AddRange(new List<Pet>
            {
                new Cat("Семен", 7, 2.5, 15.0, "Серый", false, isClaustrophobic: false),
                new Rabbit("Крош", 2, 1.5, 20.0, 10.0, true),
                new Rabbit("Лаура", 1, 1.0, 16.0, 9.5, false),
                new Dog("Хатико", 7, 10.0, 42.0, "Хаски", false, isClaustrophobic: true)
            });

            return shelters;
        }


    }
}
