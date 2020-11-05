using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using Xamarin.Forms;

using DoToo.Models;
using DoToo.Repositories;

namespace DoToo.ViewModels
{
    public class TodoItemViewModel : ViewModel
    {
        private readonly TodoItemRepository repository;
        public TodoItemViewModel(TodoItem item, TodoItemRepository repository)
        {
            this.repository = repository;
            Item = item;
        }

        public event EventHandler ItemStatusChanged;
        public TodoItem Item { get; private set; }
        public string StatusText => Item.Completed ? "Reactivate" : "Completed";

        public ICommand ToggleCompleted => new Command((arg) =>
        {
            Item.Completed = !Item.Completed;
            ItemStatusChanged?.Invoke(this, new EventArgs());
        });
        public ICommand DeleteItem => new Command(async () =>
        {
            await repository.DeleteItem(Item);
            ItemStatusChanged?.Invoke(this, new EventArgs());
        });
    }
}