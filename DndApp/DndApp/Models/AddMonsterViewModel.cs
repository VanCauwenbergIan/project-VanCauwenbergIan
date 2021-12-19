using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DndApp.Models
{
    // basic viewmodel for the bindable layout on the POST/PUT page
    public class AddMonsterViewModel
    {
        public ObservableCollection<ProficiencyAndValue> SingleProficiencies { get; set; }
        public ObservableCollection<ProficiencyAndValue> DoubleProficiencies { get; set; }
        public ObservableCollection<string> DamageVulnerabilities { get; set; }
        public ObservableCollection<string> DamageResistances { get; set; }
        public ObservableCollection<string> DamageImmunities { get; set; }
        public ObservableCollection<ConditionImmunity> ConditionImmunities { get; set; }
        public ObservableCollection<Action> Actions { get; set; }
        public ObservableCollection<Action> LegendaryActions { get; set; }
        public ObservableCollection<Action> SpecialAbilities { get; set; }

        public AddMonsterViewModel()
        {
            SingleProficiencies = new ObservableCollection<ProficiencyAndValue>();
            DoubleProficiencies = new ObservableCollection<ProficiencyAndValue>();
            DamageVulnerabilities = new ObservableCollection<string>();
            DamageResistances = new ObservableCollection<string>();
            DamageImmunities = new ObservableCollection<string>();
            ConditionImmunities = new ObservableCollection<ConditionImmunity>();
            Actions = new ObservableCollection<Action>();
            LegendaryActions = new ObservableCollection<Action>();
            SpecialAbilities = new ObservableCollection<Action>();
        }
    }
}
