using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MovieTheaterAPI.Services.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }
}
