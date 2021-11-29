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
        // original lists (so basically ascending by name)
        public List<Monster> Monsters { get; set; }

        // sorted lists (only ascending), if I'm not wrong we can just reverse the lists
        public List<Monster> MonstersByType { get; set; }
        public List<Monster> MonstersByCR { get; set; }
        public List<Monster> MonstersBySize { get; set; }
        public List<Monster> MonstersByAlignment { get; set; }
        public List<Monster> MonstersByAC { get; set; }
        public List<Monster> MonstersByHP { get; set; }
        public List<Monster> MonstersByLA { get; set; }

        public OverviewPage()
        {
            InitializeComponent();
            LoadIcons();
            Init();
        }

        private void LoadIcons()
        {
            // asigning the right images to the right "buttons" / <image> tags
            btnAdd.Source = ImageSource.FromResource("DndApp.Assets.buttonAdd.png");
            btnDropDown.Source = ImageSource.FromResource("DndApp.Assets.buttonDropRed.png");
            imgIconSearch.Source = ImageSource.FromResource("DndApp.Assets.searchIconGrey.png");
            btnFilter.Source = ImageSource.FromResource("DndApp.Assets.buttonFilterRed.png");
        }

        private void Recognizer_Tapped_sort(object sender, EventArgs e)
        {
            // puts semi-transperant overlay on the page
            popSortBy.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            popSortBy.IsVisible = true;
        }

        private async void Init()
        {
            Monsters = await MonsterRepository.GetMonstersAsync();

            rbtName.IsChecked = true;
            lvwMonsters.ItemsSource = Monsters;

            // making icons clickable so they act as buttons (put it here so you can't open the sort menu while all the data isn't there yet)
            TapGestureRecognizer recognizer_sort = new TapGestureRecognizer();

            recognizer_sort.Tapped += Recognizer_Tapped_sort;
            btnDropDown.GestureRecognizers.Add(recognizer_sort);
            lblSortBy.GestureRecognizers.Add(recognizer_sort);
        }

        private void MonsterSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // when a monster from the list is clicked, you're redirected to it's detailpage
            Monster selectedMonster = (Monster)lvwMonsters.SelectedItem;

            if (selectedMonster != null)
            {
                Navigation.PushAsync(new DetailPage(selectedMonster));
                lvwMonsters.SelectedItem = null;
            }
        }

        private void btnCancelClicked(object sender, EventArgs e)
        {
            // remove sortby menu from screen
            popSortBy.IsVisible = false;
        }

        private void btnConfirmClicked(object sender, EventArgs e)
        {
            if (rbtName.IsChecked == true)
            {
                lblSortBy.Text = "Sort by: Name";
                lvwMonsters.ItemsSource = checkForReverse(Monsters);
            }
            else if (rbtType.IsChecked == true)
            {
                if (MonstersByType == null)
                {
                    MonstersByType = MonsterMethodRepository.sortListBy(Monsters, "type");
                }
                lblSortBy.Text = "Sort by: Type";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByType);
            }
            else if (rbtChallenge.IsChecked == true)
            {
                if (MonstersByCR == null)
                {
                    MonstersByCR = MonsterMethodRepository.sortListBy(Monsters, "cr");
                }
                lblSortBy.Text = "Sort by: Challenge Rating";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByCR);
            }
            else if (rbtSize.IsChecked == true)
            {
                if (MonstersBySize == null)
                {
                    MonstersBySize = MonsterMethodRepository.sortListBy(Monsters, "size");
                }
                lblSortBy.Text = "Sort by: Size";
                lvwMonsters.ItemsSource = checkForReverse(MonstersBySize);
            }
            else if (rbtAlignment.IsChecked == true)
            {
                if (MonstersByAlignment == null)
                {
                    MonstersByAlignment = MonsterMethodRepository.sortListBy(Monsters, "alignment");
                }
                lblSortBy.Text = "Sort by: Alignment";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByAlignment);
            }
            else if (rbtArmorClass.IsChecked == true)
            {
                if (MonstersByAC == null)
                {
                    MonstersByAC = MonsterMethodRepository.sortListBy(Monsters, "ac");
                }
                lblSortBy.Text = "Sort by: Armor Class";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByAC);
            }
            else if (rbtAverageHitPoints.IsChecked == true)
            {
                if (MonstersByHP == null)
                {
                    MonstersByHP = MonsterMethodRepository.sortListBy(Monsters, "hp");
                }
                lblSortBy.Text = "Sort by: Hitpoints";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByHP);
            }
            else if (rbtLegendary.IsChecked == true)
            {
                if (MonstersByLA == null)
                {
                    MonstersByLA = MonsterMethodRepository.sortListBy(Monsters, "la");
                }
                lblSortBy.Text = "Sort by: Legendary Actions";
                lvwMonsters.ItemsSource = checkForReverse(MonstersByLA);
            }

            popSortBy.IsVisible = false;

        }

        private List<Monster> checkForReverse(List<Monster> m)
        {
            // reverses list if high to low has been checked
            if (btnHighToLow.BackgroundColor != Color.Transparent)
            {
                return m.ToArray().Reverse().ToList();
            }
            else
            {
                return m;
            }
        }

        private void btnLowToHighClicked(object sender, EventArgs e)
        {
            // basically just swapping the look of the active and non-selected button
            btnLowToHigh.BackgroundColor = Color.FromHex("#E40712");
            btnHighToLow.BackgroundColor = Color.Transparent;
        }

        private void btnHighToLowClicked(object sender, EventArgs e)
        {
            btnLowToHigh.BackgroundColor = Color.Transparent;
            btnHighToLow.BackgroundColor = Color.FromHex("#E40712");
        }
    }
}