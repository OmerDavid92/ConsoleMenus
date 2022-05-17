﻿namespace Ex04.Menus.Interfaces
{
    using System;
    using System.Collections.Generic;
    public class MainMenu : IMenuItemClickObserver
    {
        private readonly List<MainMenu> m_Menus = new List<MainMenu>();

        private readonly List<MenuItem> m_Items = new List<MenuItem>();

        public MainMenu(string i_Title)
        {
            Title = i_Title;
            MenuItem newItem = CreateItem("Exit");
            newItem.AddClickObserver(this);
        }

        public string Title { get; private set; }

        public MainMenu PrevMenu { get; private set; } = null;

        public List<MainMenu> GetMenus
        {
            get
            {
                return m_Menus;
            }
        }

        public List<MenuItem> GetItems
        {
            get
            {
                return m_Items;
            }
        }

        public MainMenu CreateSubMenu(string i_Title)
        {
            MainMenu subMenu = new MainMenu(i_Title);
            subMenu.PrevMenu = this;
            m_Menus.Add(subMenu);

            return subMenu;
        }

        public MenuItem CreateItem(string i_Title)
        {
            MenuItem item = new MenuItem(i_Title);
            m_Items.Add(item);

            return item;
        }

        private void BackOrExit()
        {
            if (PrevMenu != null)
            {
                PrevMenu.Initiate();
            }
        }

        public void ReportClick()
        {
            BackOrExit();
        }

        public bool ValidateInput(string i_UserInput, out int o_MenuSelection)
        {
            int numOfOptions = m_Menus.Count + m_Items.Count;
            bool successfulParse = int.TryParse(i_UserInput, out o_MenuSelection);

            return successfulParse && o_MenuSelection >= 0 && o_MenuSelection < numOfOptions;
        }

        public void PrintMenu()
        {
            for (int i = 0; i < m_Menus.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + 1, m_Menus[i].Title);
            }

            for (int i = 1; i < m_Items.Count; i++)
            {
                Console.WriteLine("{0} - {1}", i + m_Menus.Count, m_Items[i].Title);
            }

            Console.WriteLine("0 - {0}", m_Items[0].Title);
        }

        // [0] Exit/Back [1....X] m_Menus [X+1....END] m_Items
        public void HandleValidInput(int i_UserInput)
        {
            if (i_UserInput == 0)
            {
                m_Items[0].PerformClick();
            }
            else if (i_UserInput <= m_Menus.Count)
            {
                m_Menus[i_UserInput].Initiate();
            }
            else if (i_UserInput > m_Menus.Count)
            {
                m_Items[i_UserInput - m_Menus.Count].PerformClick();
            }
        }

        public void Initiate()
        {
            string userInput = string.Empty;
            int menuSelection = 0;
            
            Console.WriteLine(Title);
            Console.WriteLine("Please choose one of the followings:");
            PrintMenu();
            userInput = Console.ReadLine();
            
            while (!ValidateInput(userInput, out menuSelection))
            {
                Console.WriteLine("Wrong Input! Please choose one of the followings:");
                PrintMenu();
                userInput = Console.ReadLine();
            }

            HandleValidInput(menuSelection);
        }
    }
}