using Data.Models;

namespace MauiTemplate.Pages;

public partial class Page1 : ContentPage
{
	public Page1()
	{
		InitializeComponent();
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        int userId = App.UserRepo.Create(new User() { UserDetails = new UserDetails() });
        
        User user = App.UserRepo.Get(userId);

        user.UserDetails.Name = "Fred";

        App.UserRepo.Update(user);

        User usr2 = App.UserRepo.Get(userId);
    }
}