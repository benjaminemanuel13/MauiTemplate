using Data.Models;
using EntityLite.Repos;

namespace MauiTemplate;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "people.db3");

		if (File.Exists(dbPath))
		{
			File.Delete(dbPath);
		}

        builder.Services.AddSingleton<Repository<User>>(s => ActivatorUtilities.CreateInstance<Repository<User>>(s, dbPath));
        builder.Services.AddSingleton<Repository<UserDetails>>(s => ActivatorUtilities.CreateInstance<Repository<UserDetails>>(s, dbPath));

        return builder.Build();
	}
}
