﻿using System;
using System.Collections.Generic;
using System.Linq;
using WeatherApp.Commands;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public class ApplicationViewModel : BaseViewModel
    {
        #region Membres
        private BaseViewModel currentViewModel;
        private List<BaseViewModel> viewModels;
        private OpenWeatherService ows;

        #endregion

        #region Propriétés
        /// <summary>
        /// Model actuellement affiché
        /// </summary>
        public BaseViewModel CurrentViewModel
        {
            get { return currentViewModel; }
            set { 
                currentViewModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Commande pour changer la page à afficher
        /// </summary>
        public DelegateCommand<string> ChangePageCommand { get; set; }

        public List<BaseViewModel> ViewModels
        {
            get {
                if (viewModels == null)
                    viewModels = new List<BaseViewModel>();
                return viewModels; 
            }
        }
        #endregion

        public ApplicationViewModel()
        {
            ChangePageCommand = new DelegateCommand<string>(ChangePage);
           
            /// TODO 11 : Commenter cette ligne lorsque la configuration utilisateur fonctionne
            /*var apiKey = AppConfiguration.GetValue("OWApiKey");
            ows = new OpenWeatherService(apiKey);*/

            initViewModels();
        }

        #region Méthodes
        void initViewModels()
        {

            /// TemperatureViewModel setup
            var tvm = new TemperatureViewModel();
            var cvm = new ConfigurationViewModel();

            if (Properties.Settings.Default.apiKey == null){ tvm.RawText = "You need an apiKey";}
            else{ows = new OpenWeatherService(Properties.Settings.Default.apiKey); }

            /// TODO 09 : Indiquer qu'il n'y a aucune clé si le Settings apiKey est vide.
            /// S'il y a une valeur, instancié OpenWeatherService avec la clé

            tvm.SetTemperatureService(ows);
            ViewModels.Add(tvm);

            /// TODO 01 : ConfigurationViewModel Add Configuration ViewModel
            ViewModels.Add(cvm);

            CurrentViewModel = ViewModels[0];
        }

        private void ChangePage(string pageName)
        {
            /// TODO 10 : Si on a changé la clé, il faudra la mettre dans le service.
            /// 
            /// Algo
            /// Si la vue actuelle est ConfigurationViewModel
            ///   Mettre la nouvelle clé dans le OpenWeatherService
            ///   Rechercher le TemperatureViewModel dans la liste des ViewModels
            ///   Si le service de temperature est null
            ///     Assigner le service de température
            /// 
            if (CurrentViewModel.Name == "ConfigurationViewModel")
            {
                var Cvm = CurrentViewModel as ConfigurationViewModel;
                ows.SetApiKey(Cvm.ApiKey);

                for (int i = 0; i < ViewModels.Count(); i++)
                {
                    if (ViewModels[i].Name == "TemperatureViewModel")
                    {
                        var Tvm = ViewModels[i] as TemperatureViewModel;

                        if (Tvm.TemperatureService == null)
                        {
                            Tvm.SetTemperatureService(ows);
                        }
                    }
                }
            }



            /// Permet de retrouver le ViewModel avec le nom indiqé
            CurrentViewModel = ViewModels.FirstOrDefault(x => x.Name == pageName);  
        }

        #endregion
    }
}
