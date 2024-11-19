namespace Full_GRASP_And_SOLID;

public class Adaptador : TimerClient
{
    private Recipe recipe;
    public Adaptador(Recipe Recipe)
    {
        recipe = Recipe;
    }
    public void TimeOut()
    {
        recipe.CoccionTrue();
    }
}