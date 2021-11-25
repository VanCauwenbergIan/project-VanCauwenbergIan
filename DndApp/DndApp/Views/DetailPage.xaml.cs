using DndApp.Models;
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
    public partial class DetailPage : ContentPage
    {
        public Monster Monster { get; set; }
        public DetailPage(Monster selectedMonster)
        {
            Monster = selectedMonster;

            InitializeComponent();
            LoadIcons();
            LoadMonster();
        }

        private void LoadIcons()
        {
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnEdit.Source = ImageSource.FromResource("DndApp.Assets.buttonEdit.png");

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            btnBack.GestureRecognizers.Add(recognizer_return);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LoadMonster()
        {
            lblPageTitle.Text = Monster.Name;

            lblTestName.Text = Monster.Name;
            lblTestType.Text = Monster.Type;
        }

        private void btnActionsClicked(object sender, EventArgs e)
        {
            lblTestName.IsVisible = true;
            lblTestType.IsVisible = false;
            bxvActions.BackgroundColor = Color.FromHex("E40712");
            bxvStats.BackgroundColor = Color.Transparent;
        }

        private void btnStatsClicked(object sender, EventArgs e)
        {
            lblTestName.IsVisible = false;
            lblTestType.IsVisible = true;
            bxvActions.BackgroundColor = Color.Transparent;
            bxvStats.BackgroundColor = Color.FromHex("E40712");
        }
    }
}