using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        public List<Monster> Monsters { get; set; }

        public OverviewPage()
        {
            InitializeComponent();
            LoadIcons();
            LoadListView();
        }

        private void LoadIcons()
        {
            //buttonBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnAdd.Source = ImageSource.FromResource("DndApp.Assets.buttonAdd.png");
            btnDropDown.Source = ImageSource.FromResource("DndApp.Assets.buttonDropRed.png");
            imgIconSearch.Source = ImageSource.FromResource("DndApp.Assets.searchIconGrey.png");
            btnFilter.Source = ImageSource.FromResource("DndApp.Assets.buttonFilterRed.png");
        }

        private async void LoadListView()
        {
            Monsters = await MonsterRepository.GetMonstersAsync();
            lvwMonsters.ItemsSource = Monsters;
        }
    }
}