using System;
using System.Threading.Tasks;


namespace LAPhil.Settings
{
    public interface ISettingsDriver
    {
        Task<AppSettings> AppSettings();
        Task Write(AppSettings settings);
    }
}
