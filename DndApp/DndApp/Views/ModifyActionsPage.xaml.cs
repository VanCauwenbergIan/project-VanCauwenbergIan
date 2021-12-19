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
using Action = DndApp.Models.Action;

namespace DndApp.Views
{
    // if you have any questions on how this page works, it's basically the same as the ability section on the addmonsterpage
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModifyActionsPage : ContentPage
    {
        AddMonsterViewModel Obj;
        public List<Action> Actions { get; set; }
        public List<Action> LegendaryActions { get; set; }
        public List<string> OptionsUsage = new List<string> { "recharge on roll", "per day", "per long rest", "per short rest" };
        public Monster SelectedMonster { get; set; }

        public ModifyActionsPage(Monster selectedMonster, List<Monster> oMonsters, List<Monster> hbMonsters)
        {
            SelectedMonster = selectedMonster;
            InitializeComponent();
            CheckConnection();
            LoadIcons();
            LoadMonster();
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
            btnBack.Source = ImageSource.FromResource("DndApp.Assets.buttonBack.png");
            btnAddAction.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnAddLegendaryAction.Source = ImageSource.FromResource("DndApp.Assets.buttonAddRed.png");
            btnPckUsage.Source = ImageSource.FromResource("DndApp.Assets.buttonDropSmall.png");

            pckUsage.ItemsSource = OptionsUsage;

            TapGestureRecognizer recognizer_return = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_openleg = new TapGestureRecognizer();
            TapGestureRecognizer recognizer_openact = new TapGestureRecognizer();

            recognizer_return.Tapped += Recognizer_Tapped_return;
            recognizer_openleg.Tapped += Recognizer_Tapped_openleg;
            recognizer_openact.Tapped += Recognizer_Tapped_openact;
            btnBack.GestureRecognizers.Add(recognizer_return);
            btnAddAction.GestureRecognizers.Add(recognizer_openact);
            btnAddLegendaryAction.GestureRecognizers.Add(recognizer_openleg);
        }

        private void Recognizer_Tapped_return(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void Recognizer_Tapped_openleg(object sender, EventArgs e)
        {
            popActions.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            popActions.IsVisible = true;
            btnConfirmAct.ClassId = "leg";
            lblAction.Text = "Name legendary action";
        }

        private void Recognizer_Tapped_openact(object sender, EventArgs e)
        {
            popActions.BackgroundColor = Color.FromRgba(0, 0, 0, 0.5);
            popActions.IsVisible = true;
            btnConfirmAct.ClassId = "act";
            lblAction.Text = "Name action";
        }

        private void LoadMonster()
        {
            Obj = new AddMonsterViewModel();
            Actions = SelectedMonster.Actions;
            LegendaryActions = SelectedMonster.LegendaryActions;

            if (SelectedMonster.Actions != null)
            {
                Obj.Actions.Clear();
                foreach (Action action in SelectedMonster.Actions)
                {
                    Obj.Actions.Add(action);
                }

                bdlActions.BindingContext = Obj;
            }
            if (SelectedMonster.LegendaryActions != null)
            {
                Obj.LegendaryActions.Clear();
                foreach (Action action in SelectedMonster.LegendaryActions)
                {
                    Obj.LegendaryActions.Add(action);
                }

                bdlLegendaryActions.BindingContext = Obj;
            }
        }

        private void btnOptionsConfirmClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string classId = button.ClassId;

            if (classId == "leg" && ediNameAction.Text != null && ediNameAction.Text != "" && ediDescriptionAction.Text != null && ediDescriptionAction.Text != "")
            {
                int counter = 0;
                Action legAction = new Action();
                legAction.Name = ediNameAction.Text;
                legAction.Description = ediDescriptionAction.Text;

                if (LegendaryActions!= null)
                {
                    Obj.LegendaryActions.Clear();
                    foreach(Action la in LegendaryActions)
                    {
                        if (la.Name == legAction.Name)
                        {
                            counter++;
                            la.Name = legAction.Name;
                            la.Description = legAction.Description;
                            la.Usage = null;
                        }
                        Obj.LegendaryActions.Add(la);
                        bdlLegendaryActions.BindingContext = Obj;
                    }
                }
                else
                {
                    LegendaryActions = new List<Action>();
                }

                popActions.IsVisible = false;
                ediDescriptionAction.Text = null;
                ediNameAction.Text = null;
                ediNameAction.IsEnabled = true;

                if (cbxToggleUsage.IsChecked == true)
                {
                    popActionUsage.IsVisible = true;
                    popActionUsage.BindingContext = legAction;
                }
                else
                {
                    legAction.Usage = null;
                }

                if (counter == 0)
                {
                    Obj.LegendaryActions.Add(legAction);
                    LegendaryActions.Add(legAction);
                    bdlLegendaryActions.BindingContext = Obj;
                }

                cbxToggleUsage.IsChecked = false;
                btnDeleteAct.IsVisible = false;
            }
            else
            {
                int counter = 0;
                Action action = new Action();
                action.Name = ediNameAction.Text;
                action.Description = ediDescriptionAction.Text;

                if (Actions != null)
                {
                    Obj.Actions.Clear();
                    foreach (Action a in Actions)
                    {
                        if (a.Name == action.Name)
                        {
                            counter++;
                            a.Name = action.Name;
                            a.Description = action.Description;
                            a.Usage = null;
                        }
                        Obj.Actions.Add(a);
                        bdlActions.BindingContext = Obj;
                    }
                }
                else
                {
                    Actions = new List<Action>();
                }

                popActions.IsVisible = false;
                ediDescriptionAction.Text = null;
                ediNameAction.Text = null;
                ediNameAction.IsEnabled = true;

                if (cbxToggleUsage.IsChecked == true)
                {
                    popActionUsage.IsVisible = true;
                    popActionUsage.BindingContext = action;
                }
                else
                {
                    action.Usage = null;
                }

                if (counter == 0)
                {
                    Obj.Actions.Add(action);
                    Actions.Add(action);
                    bdlActions.BindingContext = Obj;
                }

                cbxToggleUsage.IsChecked = false;
                btnDeleteAct.IsVisible = false;
            }
        }

        private void btnOptionsCancelClicked(object sender, EventArgs e)
        {
            btnDeleteAct.IsVisible = false;
            popActions.IsVisible = false;
            ediNameAction.Text = null;
            ediNameAction.IsEnabled = true;
            ediDescriptionAction.Text = null;
            cbxToggleUsage.IsChecked = false;
            pckUsage.SelectedItem = null;
            entTimes.Text = null;
            entDice.Text = null;
            entMinRoll.Text = null;
        }

        private void btnUsageConfirmClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string classId = button.ClassId;
            int i = 0;
            List<Action> list = new List<Action>();

            if (lblAction.Text == "Name legendary action")
            {
                list = LegendaryActions;
            }
            else
            {
                list = Actions;
            }

            foreach (Action a in list)
            {
                if (a.Name == classId)
                {
                    if (grdUsageRoll.IsVisible == true)
                    {
                        if (entDice.Text != null && entMinRoll.Text != null && entDice.Text != "" && entMinRoll.Text != "")
                        {
                            if (int.TryParse(entMinRoll.Text, out i) == true)
                            {
                                string[] usageCheck = entDice.Text.Split('d');

                                if (int.TryParse(usageCheck[0], out i) == true && int.TryParse(usageCheck[1], out i) == true)
                                {
                                    a.Usage = new UsageObject()
                                    {
                                        Type = pckUsage.SelectedItem.ToString(),
                                        Dice = entDice.Text,
                                        MinimumValue = Int32.Parse(entMinRoll.Text)
                                    };
                                }
                            }
                        }
                    }
                    else
                    {
                        if (entTimes.Text != null && entTimes.Text != "")
                        {
                            if (int.TryParse(entTimes.Text, out i) == true)
                            {
                                a.Usage = new UsageObject()
                                {
                                    Type = pckUsage.SelectedItem.ToString(),
                                    Times = Int32.Parse(entTimes.Text)
                                };
                            }
                        }
                    }
                    break;
                }
            }

            grdUsageTime.IsVisible = false;
            grdUsageRoll.IsVisible = false;
            popActionUsage.IsVisible = false;
            pckUsage.SelectedItem = null;
            entTimes.Text = null;
            entDice.Text = null;
            entMinRoll.Text = null;
        }

        private void pckUsageTypeSelected(object sender, EventArgs e)
        {
            if (pckUsage.SelectedItem != null)
            {
                if (pckUsage.SelectedItem.ToString() == "recharge on roll")
                {
                    grdUsageRoll.IsVisible = true;
                    grdUsageTime.IsVisible = false;
                }
                else
                {
                    grdUsageTime.IsVisible = true;
                    grdUsageRoll.IsVisible = false;
                }
            }
        }

        private void EditAction(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            string classId = button.ClassId;
            List<Action> list = new List<Action>();

            if (button.Parent.ClassId == "la")
            {
                list = LegendaryActions;
                lblAction.Text = "Name legendary action";
            }
            else
            {
                list = Actions;
                lblAction.Text = "Name action";
            }

            foreach (Action a in list)
            {
                if (a.Name == classId)
                {
                    ediNameAction.Text = a.Name;
                    ediNameAction.IsEnabled = false;
                    ediDescriptionAction.Text = a.Description;

                    if (a.Usage != null)
                    {
                        cbxToggleUsage.IsChecked = true;
                        pckUsage.SelectedItem = a.Usage.Type;

                        if (pckUsage.SelectedItem.ToString() == "recharge on roll")
                        {
                            entDice.Text = a.Usage.Dice;
                            entMinRoll.Text = a.Usage.MinimumValue.ToString();
                        }
                        else
                        {
                            entTimes.Text = a.Usage.Times.ToString();
                        }
                    }
                    else
                    {
                        cbxToggleUsage.IsChecked = false;
                    }
                    btnDeleteAct.IsVisible = true;
                    popActionUsage.BindingContext = a;
                    popActions.IsVisible = true;

                    break;
                }
            }
        }

        private void DeleteAction (object sender, EventArgs e)
        {
            string name = ediNameAction.Text;
            List<Action> list = new List<Action>();

            if (lblAction.Text == "Name legendary action")
            {
                list = LegendaryActions;
            }
            else
            {
                list = Actions;
            }

            foreach (Action a in list)
            {
                if (list == LegendaryActions)
                {
                    Obj.LegendaryActions.Remove(a);
                }
                else
                {
                    Obj.Actions.Remove(a);
                }

                list.Remove(a);
                bdlLegendaryActions.BindingContext = Obj;
                bdlActions.BindingContext = Obj;
                break;
            }

            btnDeleteAct.IsVisible = false;
            popActions.IsVisible = false;
            ediNameAction.Text = null;
            ediNameAction.IsEnabled = true;
            ediDescriptionAction.Text = null;
            cbxToggleUsage.IsChecked = true;
            pckUsage.SelectedItem = null;
            entTimes.Text = null;
            entDice.Text = null;
            entMinRoll.Text = null;
        }

        private async void btnConfirmClicked (object sender, EventArgs e)
        {
            SelectedMonster.MonsterId = SelectedMonster.Name.ToLower().Replace(" ", "-");
            SelectedMonster.Actions = Actions;
            SelectedMonster.LegendaryActions = LegendaryActions;

            await MonsterRepository.PutHomebrewMonsterAsync(SelectedMonster);
            MessagingCenter.Send<Monster>(SelectedMonster, "refresh!");

            Navigation.RemovePage(Navigation.NavigationStack[1]);
            await Navigation.PopAsync();
        }
    }
}