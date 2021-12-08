using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Android.Content.ClipData;

namespace DndApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OverviewPage : ContentPage
    {
        // original lists (so basically ascending by name)
        public List<Monster> Monsters { get; set; }
        public List<Monster> HomebrewMonsters { get; set; }
        public List<Monster> CombinedMonsters { get; set; }

        // sorted lists (only ascending), we can just reverse the lists to sort from high to low. (saved, because some sorts take longer to process and this way we can just grab a list if the sort has already been used)
        public List<Monster> MonstersByType { get; set; }
        public List<Monster> MonstersByCR { get; set; }
        public List<Monster> MonstersBySize { get; set; }
        public List<Monster> MonstersByAlignment { get; set; }
        public List<Monster> MonstersByAC { get; set; }
        public List<Monster> MonstersByHP { get; set; }
        public List<Monster> MonstersByLA { get; set; }

        // current sorted list which will be filtered on
        public List<Monster> CurrentMonsters { get; set; }
        // we still want to be able to clear the filters however, without getting rid of any sorts
        public List<Monster> FilteredMonsters { get; set; }

        // save checkboxes and entries for each filter option
        public List<SubOptionCheckbox> CheckboxesType { get; set; }
        public List<SubOptionCheckbox> CheckboxesSize { get; set; }
        public List<SubOptionCheckbox> CheckboxesAlignment { get; set; }
        public List<SubOptionCheckbox> CheckboxesLegendary { get; set; }
        public List<SubOptionEntry> entriesChallenge { get; set; }
        public List<SubOptionEntry> entriesAC { get; set; }
        public List<SubOptionEntry> entriesHP { get; set; }

        // strictly speaking we wouldn't need these since we can just change the list and that's all that matters... but then you would lose UI changes (either that or hardcoding every checkbox)

        public OverviewPage()
        {
            InitializeComponent();
            LoadIcons();
            CheckConnection();
            Init();
        }

        private void CheckConnection()
        {
            Connectivity.ConnectivityChanged += ToNoNetworkPage;
        }

        private void ToNoNetworkPage(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.ConnectionProfiles.Contains(ConnectionProfile.WiFi) == false && e.ConnectionProfiles.Contains(ConnectionProfile.Cellular) == false)
            {
                Navigation.PushAsync(new NoNetworkPage());
            }
        }

        private void LoadIcons()
        {
            // asigning the right images to the right "buttons" / <image> tags
            btnAdd.Source = ImageSource.FromResource("DndApp.Assets.buttonAdd.png");
            btnDropDown.Source = ImageSource.FromResource("DndApp.Assets.buttonDropRed.png");
            btnFilter.Source = ImageSource.FromResource("DndApp.Assets.buttonFilterRed.png");
            btnCloseFilter.Source = ImageSource.FromResource("DndApp.Assets.buttonCancel.png");
            btnCloseFilterOptions.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
        }

        private void Recognizer_Tapped_sort(object sender, EventArgs e)
        {
            // puts semi-transperant overlay on the page
            popSortBy.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            popSortBy.IsVisible = true;
        }

        private void Recognizer_Tapped_openfilters(object sender, EventArgs e)
        {
            popFilterBy.IsVisible = true;
        }

        private void Recognize_Tapped_closedfilters(object sender, EventArgs e)
        {
            popFilterBy.IsVisible = false;
        }

        private void Recognize_Tapped_closesuboptions(object sender, EventArgs e)
        {
            string stringId = lblSelectedFilterOption.Text;
            List<SubOptionCheckbox> Checkboxes = new List<SubOptionCheckbox>();
            List<SubOptionEntry> Entries = new List<SubOptionEntry>();
            popFilterOptions.IsVisible = false;

            if (stringId == "Type")
            {
                Checkboxes = CheckboxesType;
            }
            else if (stringId == "Size")
            {
                Checkboxes = CheckboxesSize;
            }
            else if (stringId == "Alignment")
            {
                Checkboxes = CheckboxesAlignment;
            }
            else if (stringId == "Legendary")
            {
                Checkboxes = CheckboxesLegendary;
            }
            else if (stringId == "Challenge")
            {
                Entries = entriesChallenge;
            }
            else if (stringId == "Armor Class")
            {
                Entries = entriesAC;
            }
            else if (stringId == "Average Hitpoints")
            {
                Entries = entriesHP;
            }

            if ((stringId == "Challenge" || stringId == "Armor Class" || stringId == "Average Hitpoints") && (Entries.Where(o => o.Limit > 0).ToList().Count() > 0))
            {
                if (FilteredMonsters == null)
                {
                    FilteredMonsters = MonsterMethods.filterByEntries(CurrentMonsters, Entries, stringId);
                }
                else
                {
                    FilteredMonsters = MonsterMethods.filterByEntries(FilteredMonsters, Entries, stringId);
                }

                lvwMonsters.ItemsSource = FilteredMonsters;
            }
            else if (Checkboxes.Where(o => o.Status == true).ToList().Count() > 0)
            {
                if (FilteredMonsters == null)
                {
                    FilteredMonsters = MonsterMethods.filterByCheckboxes(CurrentMonsters, Checkboxes, stringId);
                }
                else
                {
                    FilteredMonsters = MonsterMethods.filterByCheckboxes(FilteredMonsters, Checkboxes, stringId);
                }

                lvwMonsters.ItemsSource = FilteredMonsters;
            }

            // we don't want an empty list when we don't apply any filters!
        }

        private void Recognize_Tapped_filterchosen(object sender, EventArgs e)
        {
            // assigning our ClassId / string as the title of the filter options pop-up
            string stringId = ((Frame)sender).ClassId;
            List<SubOptionCheckbox> subOptionCheckboxes = new List<SubOptionCheckbox>();
            List<SubOptionEntry> subOptionEntries = new List<SubOptionEntry>();

            lblSelectedFilterOption.Text = stringId;

            if (stringId == "Type")
            {
                if (CheckboxesType == null)
                {
                    CheckboxesType = MonsterMethods.getFilterCheckboxes(CombinedMonsters, stringId);
                }
                subOptionCheckboxes = CheckboxesType;
            }
            else if (stringId == "Size")
            {
                if (CheckboxesSize == null)
                {
                    CheckboxesSize = MonsterMethods.getFilterCheckboxes(CombinedMonsters, stringId);
                }
                subOptionCheckboxes = CheckboxesSize;
            }
            else if (stringId == "Alignment")
            {
                if (CheckboxesAlignment == null)
                {
                    CheckboxesAlignment = MonsterMethods.getFilterCheckboxes(CombinedMonsters, stringId);
                }
                subOptionCheckboxes = CheckboxesAlignment;
            }
            else if (stringId == "Legendary")
            {
                if (CheckboxesLegendary == null)
                {
                    CheckboxesLegendary = MonsterMethods.getFilterCheckboxes(CombinedMonsters, stringId);
                }
                subOptionCheckboxes = CheckboxesLegendary;
            }
            else if (stringId == "Challenge")
            {
                if (entriesChallenge == null)
                {
                    entriesChallenge = MonsterMethods.getFilterEntries();
                }
                subOptionEntries = entriesChallenge;
            }
            else if (stringId == "Armor Class")
            {
                if (entriesAC == null)
                {
                    entriesAC = MonsterMethods.getFilterEntries();
                }
                subOptionEntries = entriesAC;
            }
            else if (stringId == "Average Hitpoints")
            {
                if (entriesHP == null)
                {
                   entriesHP = MonsterMethods.getFilterEntries();
                }
                subOptionEntries = entriesHP;
            }

            if (stringId == "Challenge" || stringId == "Armor Class" || stringId == "Average Hitpoints")
            {
                lvwFilterEntries.ItemsSource = subOptionEntries;
                lvwFilterEntries.IsVisible = true;
                lvwFilterCheckboxes.IsVisible = false;
            }
            else
            {
                lvwFilterCheckboxes.ItemsSource = subOptionCheckboxes;
                lvwFilterEntries.IsVisible = false;
                lvwFilterCheckboxes.IsVisible = true;
            }

            popFilterOptions.IsVisible = true;
        }

        private async void Init()
        {
            Monsters = await MonsterRepository.GetMonstersAsync();
            HomebrewMonsters = await MonsterRepository.GetHomebrewMonsterAsync();

            rbtName.IsChecked = true;
            CurrentMonsters = Monsters.Concat(HomebrewMonsters).ToList();
            CurrentMonsters = CurrentMonsters.OrderBy(o => o.Name).ToList();
            CombinedMonsters = CurrentMonsters;
            grdActivity.IsVisible = false;
            actListview.IsRunning = false;
            lvwMonsters.ItemsSource = CurrentMonsters;

            // making icons clickable so they act as buttons (put it here so you can't open the sort menu while all the data isn't there yet, which would cause a crash if a sort or filter was applied)
            TapGestureRecognizer recognizer_sort = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_openfilters = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_closefilters = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_filterchosen = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_closesuboptions = new TapGestureRecognizer();

            recognizer_sort.Tapped += Recognizer_Tapped_sort;
            recognizer_openfilters.Tapped += Recognizer_Tapped_openfilters;
            recognizer_closefilters.Tapped += Recognize_Tapped_closedfilters;
            recognizer_filterchosen.Tapped += Recognize_Tapped_filterchosen;
            recognizer_closesuboptions.Tapped += Recognize_Tapped_closesuboptions;

            // a simple way to pass a string along with the frames that can be accessed by the event handler
            frmFilterType.ClassId = "Type";
            frmFilterChallenge.ClassId = "Challenge";
            frmFilterSize.ClassId = "Size";
            frmFilterAlignment.ClassId = "Alignment";
            frmFilterArmorClass.ClassId = "Armor Class";
            frmFilterAverageHP.ClassId = "Average Hitpoints";
            frmFilterLegendary.ClassId = "Legendary";

            btnDropDown.GestureRecognizers.Add(recognizer_sort);
            lblSortBy.GestureRecognizers.Add(recognizer_sort);
            btnFilter.GestureRecognizers.Add(recognizer_openfilters);
            btnCloseFilterOptions.GestureRecognizers.Add(recognizer_closesuboptions);
            btnCloseFilter.GestureRecognizers.Add(recognizer_closefilters);
            frmFilterType.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterChallenge.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterSize.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterAlignment.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterArmorClass.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterAverageHP.GestureRecognizers.Add(recognizer_filterchosen);
            frmFilterLegendary.GestureRecognizers.Add(recognizer_filterchosen);
        }

        private void MonsterSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // when a monster from the list is clicked, you're redirected to it's detailpage
            Monster selectedMonster = (Monster)lvwMonsters.SelectedItem;

            if (selectedMonster != null)
            {
                Navigation.PushAsync(new DetailPage(selectedMonster, Monsters, HomebrewMonsters));
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
                // original list is laready sorted alphabetically
                lblSortBy.Text = "Sort by: Name";
                CurrentMonsters = checkForReverse(CombinedMonsters);

                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(FilteredMonsters.OrderBy(o => o.Name).ToList());
                }
            }
            else if (rbtType.IsChecked == true)
            {
                if (MonstersByType == null)
                {
                    MonstersByType = MonsterMethods.sortListBy(CombinedMonsters, "type");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "type"));
                }
                lblSortBy.Text = "Sort by: Type";
                CurrentMonsters = checkForReverse(MonstersByType);
            }
            else if (rbtChallenge.IsChecked == true)
            {
                if (MonstersByCR == null)
                {
                    MonstersByCR = MonsterMethods.sortListBy(CombinedMonsters, "cr");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "cr"));
                }
                lblSortBy.Text = "Sort by: Challenge Rating";
                CurrentMonsters = checkForReverse(MonstersByCR);
            }
            else if (rbtSize.IsChecked == true)
            {
                if (MonstersBySize == null)
                {
                    MonstersBySize = MonsterMethods.sortListBy(CombinedMonsters, "size");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "size"));
                }
                lblSortBy.Text = "Sort by: Size";
                CurrentMonsters = checkForReverse(MonstersBySize);
            }
            else if (rbtAlignment.IsChecked == true)
            {
                if (MonstersByAlignment == null)
                {
                    MonstersByAlignment = MonsterMethods.sortListBy(CombinedMonsters, "alignment");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "alignment"));
                }
                lblSortBy.Text = "Sort by: Alignment";
                CurrentMonsters = checkForReverse(MonstersByAlignment);
            }
            else if (rbtArmorClass.IsChecked == true)
            {
                if (MonstersByAC == null)
                {
                    MonstersByAC = MonsterMethods.sortListBy(CombinedMonsters, "ac");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "ac"));
                }
                lblSortBy.Text = "Sort by: Armor Class";
                CurrentMonsters = checkForReverse(MonstersByAC);
            }
            else if (rbtAverageHitPoints.IsChecked == true)
            {
                if (MonstersByHP == null)
                {
                    MonstersByHP = MonsterMethods.sortListBy(CombinedMonsters, "hp");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "hp"));
                }
                lblSortBy.Text = "Sort by: Hitpoints";
                CurrentMonsters = checkForReverse(MonstersByHP);
            }
            else if (rbtLegendary.IsChecked == true)
            {
                if (MonstersByLA == null)
                {
                    MonstersByLA = MonsterMethods.sortListBy(CombinedMonsters, "la");
                }
                if (FilteredMonsters != null)
                {
                    FilteredMonsters = checkForReverse(MonsterMethods.sortListBy(FilteredMonsters, "la"));
                }
                lblSortBy.Text = "Sort by: Legendary Actions";
                CurrentMonsters = checkForReverse(MonstersByLA);
            }

            if (FilteredMonsters == null)
            {
                lvwMonsters.ItemsSource = CurrentMonsters;
            }
            else
            {
                lvwMonsters.ItemsSource = FilteredMonsters;
            }
            popSortBy.IsVisible = false;

        }

        private List<Monster> checkForReverse(List<Monster> m)
        {
            // reverses list if high to low has been checkedcurrentMonsters
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

        private void btnClearAllFiltersClicked(object sender, EventArgs e)
        {
            lvwMonsters.ItemsSource = CurrentMonsters;
            FilteredMonsters = null;

            CheckboxesType = null;
            CheckboxesSize = null;
            CheckboxesAlignment = null;
            CheckboxesLegendary = null;
            entriesAC = null;
            entriesChallenge = null;
            entriesHP = null;
        }
    }
}