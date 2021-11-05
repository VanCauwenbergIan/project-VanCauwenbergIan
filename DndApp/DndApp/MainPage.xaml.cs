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
            Debug.WriteLine($"Amount of monsters read {monsters.Count()}");
            foreach (Monster monster in monsters)
            {
                Debug.WriteLine(monster);
            }
            Debug.WriteLine("Done!!!");

            // picking some random monsters and to check their properties
            System.Random rnd = new System.Random();
            List<Monster> randomMonsters = new List<Monster>();

            for (int i =  0; i < 5; i++)
            {
                int index = rnd.Next(monsters.Count());
                randomMonsters.Add(monsters[index]);
            }

            // put breakpoint here to read list
            Debug.WriteLine(randomMonsters);
            // from the 5 monsters I compared with the JSON, most bugs should be gone now
        }
    }
}
