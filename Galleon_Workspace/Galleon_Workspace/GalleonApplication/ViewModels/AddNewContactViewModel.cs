using GalleonApplication.Events;
using GalleonApplication.Extra;
using GalleonApplication.Extra.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using GalleonApplication.Managers;

namespace GalleonApplication.ViewModels {
    
    public class AddNewContactViewModel : IPropertyChanged {

        private string contactName;
        private string address;
        private string phone;
        private CountryEntity country;
        private CityEntity city;

        private CountryEntities countries;
        private CityEntities cities;
        
        //will be used as ButtonCommandParamater
        private ContactEntity contactEntity;

        public CityEntities Cities {
            get {
                return cities;
            }
            set {
                setProperty(ref cities, value);
            }
        }
        
        public CountryEntities Countries {
            get {
                return countries;
            }
            set {
                setProperty(ref countries, value);
            }
        }
        
        public CityEntity City {
            get {
                return city;
            }
            set {
                setProperty(ref city, value);
                if(!city.IsNullOrEmpty()) {
                    contactEntity.City = contactEntity.City.IsNullOrEmpty() ? city : contactEntity.City.CityID != city.CityID ? city : contactEntity.City;
                }
            }
        }

        public CountryEntity Country {
            get {
                return country;
            }
            set {
                setProperty(ref country, value);
                if(!value.IsEquals(country)) {
                    contactEntity.Country = value;
                }
                if(!value.IsEquals(country) && !value.IsNullOrEmpty()) {
                    //fetch cities for country
                    ShowCityProgress = true;
                    cityCancelation = endpointClient.queryCities(value.CountryID)
                            .ToResponse<List<CityEntity>>()
                            .SubscribeOn(ThreadPoolScheduler.Instance)
                            .ObserveOnDispatcher(DispatcherPriority.Background)
                            .Subscribe(OnSuccessCities, OnErrorCities, () => {
                                ShowCityProgress = false;
                            });
                }
            }
        }

        public string Phone {
            get {
                return phone;
            }
            set {
                setProperty(ref phone, value);
                if(!value.IsEquals(phone)) {
                    contactEntity.Phone = value;
                }
            }
        }

        public string Address {
            get {
                return address;
            }
            set {
                setProperty(ref address, value);
                if(!value.IsEquals(address)) {
                    contactEntity.Address = value;
                }
            }
        }

        public string ContactName {
            get {
                return contactName;
            }
            set {
                setProperty(ref contactName, value);
                if(!value.IsEquals(contactName)) {
                    contactEntity.ContactName = value;
                }
            }
        }        
        
        public string Title {
            get {
                return Properties.Resources.DialogAddNewContactTitle;
            }           
        }

        public ContactEntity ContactEntity {
            get {
                return contactEntity;
            }
            set {
                setProperty(ref contactEntity, value);
                if(!value.IsEquals(contactEntity)) {
                    //set for edit
                    ContactName = value.ContactName;
                    Address = value.Address;
                    Phone = value.Phone;
                }
            }
        }

        private bool showCountryProgress;
        public bool ShowCountryProgress { 
            get {
                return showCountryProgress;
            }
            set {
                setProperty(ref showCountryProgress, value);
            }
        }

        private bool showCityProgress;
        public bool ShowCityProgress {
            get {
                return showCityProgress;
            }
            set {
                setProperty(ref showCityProgress, value);
            }
        }

        private Action<Response<List<CountryEntity>>> OnSuccessCountries {
            get {
                return x => {
                    if(x.Code == ResponseCode.OK) {
                        List<CountryEntity> data = x.Data;
                        if(Countries.IsNullOrEmpty()) {
                            Countries = new CountryEntities();
                        }
                        if(!data.IsNullOrEmpty()) {
                            data.ForEach(d => Countries.Add(d));
                        }
                        //if we editing then we need to assing it back
                        CountryEntity selectedCountry = contactEntity.Country;
                        if(!selectedCountry.IsNullOrEmpty()) {
                            Country = Countries.FirstOrDefault(y => y.CountryID == selectedCountry.CountryID);
                        }
                    } else {
                        x.PersistResponseError();
                    }
                };
            }
        }

        private Action<Exception> OnErrorCountries {
            get {
                return error => {
                    BusManager.Send(new SnackbarEvent() {
                        textMessage = Properties.Resources.UnexpectedErrorText,
                        withDuration = null,
                        isCloseEvent = false
                    });
                    //persist data on db
                    error.PersistException();
                };
            }
        }

        private Action<Response<List<CityEntity>>> OnSuccessCities {
            get {
                return x => {                   
                    if(x.Code == ResponseCode.OK) {
                        List<CityEntity> cities = x.Data;
                        if(Cities.IsNullOrEmpty()) {
                            Cities = new CityEntities();
                        } else {
                            Cities.Clear();
                        }
                        if(!cities.IsNullOrEmpty()) {
                            cities.ForEach(c => Cities.Add(c));
                        }
                        //if we are editting we need to assing it back
                        CityEntity selectedCity = contactEntity.City;
                        if(!selectedCity.IsNullOrEmpty()) {
                            City = Cities.FirstOrDefault(y => y.CityID == selectedCity.CityID);
                        }
                    } else {
                        x.PersistResponseError();
                    }
                };
            }
        }

        private Action<Exception> OnErrorCities {
            get {
                return error => {                   
                    BusManager.Send(new SnackbarEvent() {
                        textMessage = Properties.Resources.UnexpectedErrorText,
                        withDuration = null,
                        isCloseEvent = false
                    });
                    //persisit on db
                    error.PersistException();
                };
            }
        }

        private IEndpointClient endpointClient;

        private IDisposable countryCancelation;
        private IDisposable cityCancelation;

        public AddNewContactViewModel(IEndpointClient endpointClient) {
            this.endpointClient = endpointClient;            
        }

        public void OnStart() {            
            ShowCountryProgress = true;
            if(ContactEntity.IsNullOrEmpty()) {
                ContactEntity = new ContactEntity();
            }
            //fetch countries for now
            countryCancelation = endpointClient.queryCountries()
                .ToResponse<List<CountryEntity>>()
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOnDispatcher(DispatcherPriority.Background)
                .Subscribe(OnSuccessCountries, OnErrorCountries, () => {
                    ShowCountryProgress = false;
                });
        }

        public void OnStop() {
            ShowCountryProgress = false;
            if(!countryCancelation.IsNullOrEmpty()) {
                countryCancelation.Dispose();
                countryCancelation = null;
            }
            ShowCityProgress = false;
            if(!cityCancelation.IsNullOrEmpty()) {
                cityCancelation.Dispose();
                cityCancelation = null;
            }            
        }
    }
}
