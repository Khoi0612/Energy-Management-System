using EMS.Core.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace EMS.Core.Interfaces
{
    internal interface IEnergyReadingRepository
    {
        Task SaveReadingAsync(EnergyReading reading);
        Task<ObservableCollection<EnergyReading>> GetReadingsAsync(DateTime? startDate, DateTime? endDate, string frequency);
    }
}
