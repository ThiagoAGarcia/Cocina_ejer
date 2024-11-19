namespace Full_GRASP_And_SOLID;

public class TemporizadorDeCoccion : TimerClient
{
    private Recipe receta;
    public TemporizadorDeCoccion(Recipe recipe)
    {
        this.receta = recipe;
    }
    public void TimeOut()
    {
        receta.CoccionTrue();
    }
}