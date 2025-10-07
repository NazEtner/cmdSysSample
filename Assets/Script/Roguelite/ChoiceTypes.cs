using System;
using System.Collections.Generic;

namespace Nananami.Rougelite
{
    [Serializable]
    public class ChoiseData
    {
        public List<Choise> choises;
    }


    [Serializable]
    public class Choise
    {
        public int price;
        public double priceIncreaseRate;
        public string text;
        public int levelRequired;
        public int maxDuplicate;
        public List<Message> messages;
    }

    [Serializable]
    public class Message
    {
        public string name;
        public string contents;
    }
}