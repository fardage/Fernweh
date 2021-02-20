using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Fernweh.Data;
using Newtonsoft.Json;

namespace Fernweh.Models
{
    public class Item : INotifyPropertyChanged
    {
        private bool _packed;

        [JsonIgnore] public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        [JsonIgnore]
        public bool Packed
        {
            get => _packed;
            set
            {
                _packed = value;
                OnPropertyChanged(nameof(Packed));
                _ = DataStore.UpdateItemAsync(this);
            }
        }

        [JsonIgnore] [NotMapped] public bool IsEnabled { get; set; } = true;
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}