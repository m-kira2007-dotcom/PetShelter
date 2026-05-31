using System;
using System.Collections.Generic;
using System.Windows;
using Model.Core; 
using Model.Data; //  AllInformation(все именна параметры питомцев там)

namespace PetShelter
{
    public partial class TableWindow : Window
    {
        
        private List<Pet> _displayedPets;      // Текущие строки в таблице
        private List<Shelter> _allShelters;    // Ссылка на общую базу приютов
        private int _selectedShelterIndex;     // Выбранный индекс из главного меню


        public TableWindow(List<Pet> petsForTable, List<Shelter> allShelters, int shelterIndex)
        {
            InitializeComponent();

            _displayedPets = petsForTable;
            _allShelters = allShelters;
            _selectedShelterIndex = shelterIndex;

            dgPets.ItemsSource = _displayedPets;

            //  Автосохранение отчета подборки при открытии окна результатов
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            AllInformation.CurrentSerializer.SerializeSelection(desktopPath, _displayedPets);
        }

        //  ADOPT NEW PET
        private void BtnAddPet_Click(object sender, RoutedEventArgs e)
        {
            Shelter targetShelter = null;
            if (_selectedShelterIndex > 0) targetShelter = _allShelters[_selectedShelterIndex - 1];
            else if (_allShelters != null && _allShelters.Count > 0) targetShelter = _allShelters[0];

            if (targetShelter == null) return;

           
            string[] coolNames = {
                "Кира", "Зефир", "Пончик", "Коржик", "Чак",
                "Гарфилд", "Вика", "Абу", "Рокки", "Маркус",
                "Барон", "Пират", "Арина", "Пушок", "Кокос"
            };

            // Выбираем случайное имя из нашего массива
            Random rand = new Random();
            string luckyName = coolNames[rand.Next(coolNames.Length)];

            // Генерируем случайный парраметр 
            int randomAge = rand.Next(1, 9);
            double randomWeight = Math.Round(2.5 + rand.NextDouble() * 4, 1);
            double randomHeight = Math.Round(18.0 + rand.NextDouble() * 12, 1);

            // СЛУЧАЙНЫЙ ВЫБОР ВИДА: генерируем число от 0 до 3
            
            Pet newPet = null;
            // делаем так что при выборе фильтра например собака при добавлении животного добавлялась только собака(не любое другое животное)
            if (_displayedPets != null && _displayedPets.Count > 0)
            {
                // Берем тип самого первого животного из таблицы 
                Type currentFilterType = _displayedPets[0].GetType();

                if (currentFilterType == typeof(Cat))
                    newPet = new Cat(luckyName, randomAge, randomWeight, randomHeight, "Рыжий", isLazy: true, isClaustrophobic: false);
                else if (currentFilterType == typeof(Dog))
                    newPet = new Dog(luckyName, randomAge, randomWeight, randomHeight, "Дворняжка", knowsCommands: true, isClaustrophobic: false);
                else if (currentFilterType == typeof(Rabbit))
                    newPet = new Rabbit(luckyName, randomAge, randomWeight, randomHeight, 12.5, true, isClaustrophobic: false);
                else if (currentFilterType == typeof(Parrot))
                    newPet = new Parrot(luckyName, randomAge, randomWeight, randomHeight,  "Мужской", true, isClaustrophobic: false);
            }


            // СЛУЧАЙНЫЙ ВЫБОР ВИДА: генерируем число от 0 до 3
            if (newPet == null)
            {
                int randomType = rand.Next(0, 4);
                if (randomType == 0) newPet = new Cat(luckyName, randomAge, randomWeight, randomHeight, "Рыжий", isLazy: true, isClaustrophobic: false);
                else if (randomType == 1) newPet = new Dog(luckyName, randomAge, randomWeight, randomHeight, "Дворняжка", knowsCommands: true, isClaustrophobic: false);
                else if (randomType == 2) newPet = new Rabbit(luckyName, randomAge, randomWeight, randomHeight,  12.5, true, isClaustrophobic: false);
                else newPet = new Parrot(luckyName, randomAge, randomWeight, randomHeight, "Мужской", true, isClaustrophobic: false);
            }


            // Отправляем на проверку в приют (включая проверку делегата Func)
            if (targetShelter.AddPet(newPet))
            {
                _displayedPets.Add(newPet); // Добавляем на экран

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                AllInformation.SaveData(desktopPath, _allShelters); // Сохраняем в JSON/XML

                RefreshGrid(); // Обновляем таблицу
                MessageBox.Show($"🐾 Питомец '{luckyName}' успешно принят в приют \"{targetShelter.Name}\"!", "Ураааа", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Не удалось принять животное! Возможно, приют переполнен.", "Ограничение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        //  TAKE ME HOME(удалить забрать 
        private void BtnRemovePet_Click(object sender, RoutedEventArgs e)
        {
            Pet selectedPet = (Pet)dgPets.SelectedItem;

            if (selectedPet == null)
            {
                MessageBox.Show("Пожалуйста, сначала выберите животное в таблице кликом мышки!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Shelter ownerShelter = null;
            foreach (Shelter shelter in _allShelters)
            {
                if (shelter.Pets.Contains(selectedPet))
                {
                    ownerShelter = shelter;
                    break;
                }
            }

            if (ownerShelter != null)
            {
                ownerShelter.RemovePet(selectedPet);
                _displayedPets.Remove(selectedPet);

                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                AllInformation.SaveData(desktopPath, _allShelters);

                RefreshGrid();
                MessageBox.Show($"🎉 Урааа,отлично! Животное '{selectedPet.Name}' успешно забрали из приюта \"{ownerShelter.Name}\"!", "Take Me Home", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RefreshGrid()
        {
            dgPets.ItemsSource = null;//отвязывает старый список 

            // Заставляем DataGrid принудительно обновить макет колонок перед выводом
            dgPets.UpdateLayout();//заставляет Windows мгновенно перерисовать интерфейс появление надписей «Кот», «Собака»

            dgPets.ItemsSource = _displayedPets;// загружает в таблицу уже обновленный список животных
        }
    }
}
