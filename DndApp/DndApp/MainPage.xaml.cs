using DndApp.Models;
using DndApp.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DndApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            TestMonsterRepo();
        }

        private async Task TestMonsterRepo()
        {
            List<Monster> monsters = await MonsterRepository.GetMonstersAsync();
            Debug.WriteLine("Testing GetMonstersAsync");
            foreach (Monster monster in monsters)
            {
                Debug.WriteLine(monster);
            }
            Debug.WriteLine("Done!!!");
        }
    }
}
