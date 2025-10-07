using System;
using System.Collections.Generic;

namespace Nananami.Rougelite
{
    [Serializable]
    public struct ChoiseData
    {
        public List<Choise> choises { get; set; }
    }


    [Serializable]
    public struct Choise
    {
        public int price { get; set; }
        public double priceIncreaseRate { get; set; }
        public string text { get; set; }
        public int levelRequired { get; set; }
        public int maxDuplicate { get; set; }
        public List<Message> messages { get; set; }
    }

    [Serializable]
    public struct Message
    {
        public string name { get; set; }
        public string contents { get; set; }
    }
}