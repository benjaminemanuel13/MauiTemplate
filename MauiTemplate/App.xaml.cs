using Business;
using Data.Models;
using EntityLite.Repos;

namespace MauiTemplate;

public partial class App : Application
{
    public static Repository<User> UserRepo { get; private set; }
    public static Repository<UserDetails> UserDetailsRepo { get; private set; }


    public App(Repository<User> userRepo, Repository<UserDetails> userDetailsRepo)
	{
		InitializeComponent();

        UserRepo = userRepo;
        UserDetailsRepo = userDetailsRepo;

        MainPage = new MainPage();
	}
}
