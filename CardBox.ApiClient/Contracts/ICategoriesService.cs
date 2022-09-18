using CardBox.ApiClient.Models;

namespace CardBox.ApiClient.Contracts;

public interface ICategoriesService
{
    public List<Category> GetCategories();
}