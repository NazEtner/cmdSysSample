using System;
using System.Collections.Generic;

namespace Nananami.Rougelite
{
    [Serializable]
    public class ChoiceData
    {
        public List<Choice> choices;
    }


    [Serializable]
    public class Choice
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