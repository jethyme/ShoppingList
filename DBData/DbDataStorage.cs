using Core.Interfaces;
using Core.Models;
using DBData.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBData
{
    public class DbDataStorage : IDataStorage
    {
        public async Task SaveDataAsync(IEnumerable<ShoppingList> lists)
        {
            using var context = new AppDbContext();

            var dbLists = lists.Select(list => new DBData.Models.ShoppingList
            {
                Name = list.Name,
                Items = list.Items.Select(item => new DBData.Models.ShoppingItem
                {
                    Name = item.Name,
                    Quantity = item.Quantity,
                    IsPurchased = item.IsPurchased,
                    PurchaseTime = item.PurchaseTime,
                    Parameters = item.Parameters.Select(param => new DBData.Models.ShoppingItemParameter
                    {
                        Key = param.Key,
                        Value = param.Value}).ToList()
                }).ToList()
            }).ToList();

            context.ShoppingLists.AddRange(dbLists);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ShoppingList>> LoadDataAsync()
        {
            using var context = new AppDbContext();
            var dbLists = await context.ShoppingLists
                .Include(sl => sl.Items)
                .ThenInclude(si => si.Parameters) // Включаем параметры
                .ToListAsync();

            return dbLists.Select(list => new Core.Models.ShoppingList
            {
                Id = list.Id,
                Name = list.Name,
                Items = list.Items.Select(item => new Core.Models.ShoppingItem
                {
                    Id = item.Id,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    IsPurchased = item.IsPurchased,
                    PurchaseTime = item.PurchaseTime,
                    Parameters = item.Parameters.ToDictionary(param => param.Key, param => param.Value) // Преобразуем параметры обратно в словарь
                }).ToList()
            }).ToList();
        }
    }
}