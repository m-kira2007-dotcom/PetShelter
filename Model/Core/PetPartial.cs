using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Core//В PetPartial.cs пишется только алгоритмическая логика: методы фильтрации, правила клаустрофобии 
{
    public abstract partial class Pet
    {
        public Pet(string name, int age, double weight, double height, bool isClaustrophobic)
        {
            Name = name;
            Age = age;
            Weight = weight;
            Height = height;
            IsClaustrophobic = isClaustrophobic;
        }
       
    }
    public partial class Cat
    {
        public Cat(string name, int age, double weight, double height, string color, bool isLazy, bool isClaustrophobic = true) : base(name, age, weight, height, isClaustrophobic)
        {
            FurColor = color;
            IsLazy = isLazy;
        }
    }
    public partial class Dog
    {
        public Dog(string name, int age, double weight, double height, string breed, bool knowsCommands, bool isClaustrophobic = false) : base(name, age, weight, height, isClaustrophobic)
        {
            Breed = breed;
            KnowsCommands = knowsCommands;
        }
    }
    public partial class Rabbit
    {
        public Rabbit(string name, int age, double weight, double height, double earLength, bool isDomestic, bool isClaustrophobic = true) : base(name, age, weight, height, isClaustrophobic)
        {
            EarLength = earLength;
            IsDomestic = isDomestic;
        }
    }
    public partial class Parrot
    {
        public Parrot(string name, int age, double weight, double height, string gender, bool isTalking, bool isClaustrophobic = false) : base(name, age, weight, height, isClaustrophobic)
        {
            Gender = gender;
            IsTalking = isTalking;
        }
    }
}
