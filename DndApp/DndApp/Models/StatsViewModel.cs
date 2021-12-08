using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DndApp.Models
{
    public class StatsModel
    {
        public string Ability { get; set; }

        public double Value { get; set; }

        public StatsModel(string xValue, double yValue)
        {
            Ability = xValue;
            Value = yValue;
        }
    }

    public class StatsViewModel
    {
        public ObservableCollection<StatsModel> DataST { get; set; }
        public ObservableCollection<StatsModel> DataAS { get; set; }

        public StatsViewModel()
        {
            DataST = new ObservableCollection<StatsModel>();
            DataAS = new ObservableCollection<StatsModel>();
        {
        };
        }
    }
}
