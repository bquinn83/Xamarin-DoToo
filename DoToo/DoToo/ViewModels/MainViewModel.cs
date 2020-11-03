﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

using DoToo.Models;
using DoToo.Repositories;
using DoToo.Views;

namespace DoToo.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly TodoItemRepository repository;

        public ObservableCollection<TodoItemViewModel> Items { get; set; }

        public MainViewModel(TodoItemRepository repository)
        {
            repository.OnItemAdded += (sender, item) => Items.Add(CreateTodoItemViewModel(item));
            repository.OnItemUpdated += (sender, item) => Task.Run(async () => await LoadData());
            this.repository = repository;
            Task.Run(async () => await LoadData());
        }

        public ICommand AddItem => new Command(async () =>
        {
            var itemView = Resolver.Resolve<ItemView>();
            await Navigation.PushAsync(itemView);
        });
        public TodoItemViewModel SelectedItem
        {
            get { return null; }
            set
            {
                Device.BeginInvokeOnMainThread(async () => await NavigateToItem(value));
                RaisePropertyChanged(nameof(SelectedItem));
            }
        }
        private async Task NavigateToItem(TodoItemViewModel item)
        {
            if (item == null)
            {
                return;
            }

            var itemView = Resolver.Resolve<ItemView>();
            var vm = itemView.BindingContext as ItemViewModel;
            vm.Item = item.Item;

            await Navigation.PushAsync(itemView);
        }

        private async Task LoadData()
        {
            var items = await repository.GetItems();
            var itemViewModels = items.Select(i => CreateTodoItemViewModel(i));
            Items = new ObservableCollection<TodoItemViewModel>(itemViewModels);
        }
        private TodoItemViewModel CreateTodoItemViewModel(TodoItem item)
        {
            var itemViewModel = new TodoItemViewModel(item);
            itemViewModel.ItemStatusChanged += ItemStatusChanged;
            return itemViewModel;
        }
        private void ItemStatusChanged(object sender, EventArgs e)
        {

        }
    }
}
