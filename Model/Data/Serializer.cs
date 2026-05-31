using System;
using System.Collections.Generic;
using Model.Core;

namespace Model.Data
{
    // ОБЩИЙ АБСТРАКТНЫЙ КЛАСС ДЛЯ СЕРИАЛИЗАЦИИ
    public abstract class Serializer
    {
        // Расширение файла (например, ".json" или ".xml")
        public abstract string Extension { get; }

        // Абстрактные методы, которые обязаны реализовать наследники
        public abstract void Serialize(string folderPath, List<Shelter> data);
        public abstract List<Shelter> Deserialize(string folderPath);

        // Метод для записи отдельных подборок-отчетов из окон таблиц
        public abstract void SerializeSelection(string folderPath, List<Pet> pets);
    }
}
