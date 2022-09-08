namespace SerresApp.Interfaces
{
    /// <summary>
    /// Interface for checking if an image exists
    /// </summary>
    public interface IImageChecker
    {
        bool DoesImageExist(string image);
    }
}
